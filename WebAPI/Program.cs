using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.Ioc;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.JWT;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // OpenAPI Modelleri için gerekli
using Scalar.AspNetCore;        // Scalar için gerekli
using System.Diagnostics;
using Core.Extensions.Exceptions;
using Business.Extensions;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------------------------------------------------
            // 1. AUTOFAC KURULUMU
            // -------------------------------------------------------------------------
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    containerBuilder.RegisterModule(new AutofacBusinessModule());
                });

            // -------------------------------------------------------------------------
            // 2. SERVİS KAYITLARI
            // -------------------------------------------------------------------------
            builder.Services.AddControllers();

            // .NET 9 NATIVE OPENAPI AYARLARI
            // Swashbuckle yerine Microsoft'un kendi kütüphanesini kullanıyoruz.
            // .NET 9 NATIVE OPENAPI AYARLARI
            builder.Services.AddOpenApi("v1", options =>
            {
                // 1. ADIM: "Bearer" şemasını dökümana genel bileşen olarak ekle (Tanım)
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Token giriniz."
                    };

                    document.Components ??= new OpenApiComponents();
                    // Eğer daha önce eklenmediyse ekle
                    if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
                    {
                        document.Components.SecuritySchemes.Add("Bearer", securityScheme);
                    }

                    return Task.CompletedTask;
                });

                // 2. ADIM: Her endpoint'i kontrol et ve sadece [Authorize] olanlara kilit koy
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    // Endpoint üzerindeki metadata'ları (Attribute'leri) çekiyoruz
                    var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                    // [Authorize] var mı?
                    bool hasAuthorize = metadata.Any(m => m is Microsoft.AspNetCore.Authorization.IAuthorizeData);

                    // [AllowAnonymous] var mı? (Authorize olsa bile AllowAnonymous varsa kilit koymamalıyız)
                    bool hasAnonymous = metadata.Any(m => m is Microsoft.AspNetCore.Authorization.IAllowAnonymous);

                    // Eğer Authorize varsa VE AllowAnonymous yoksa -> KİLİT EKLE
                    if (hasAuthorize && !hasAnonymous)
                    {
                        operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };

                        // Opsiyonel: Swagger'da 401 hatasını da dökümante etmek istersen
                        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                    }

                    return Task.CompletedTask;
                });
            });
            // CORS Ayarları
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(corsBuilder =>
                {
                    corsBuilder.SetIsOriginAllowed(origin =>
                    {
                        if (string.IsNullOrEmpty(origin)) return false;
                        try
                        {
                            var uri = new Uri(origin);
                            var host = uri.Host;
                            // Güvenlik: Canlı ortamda sadece kendi domainlerinizi ekleyin
                            if (host == "localhost" || host == "127.0.0.1") return true;
                            return false;
                        }
                        catch { return false; }
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });

            // MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Business.Features.CQRS.Auth.Login.LoginHandler).Assembly));

            // Standart Servisler
            builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
            builder.Services.AddScoped<IKullaniciDal, EFKullaniciDal>();

            // -------------------------------------------------------------------------
            // 3. JWT TOKEN VE AUTHENTICATION
            // -------------------------------------------------------------------------
            var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

            // -------------------------------------------------------------------------
            // 4. DEPENDENCY RESOLVERS
            // -------------------------------------------------------------------------
            builder.Services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule()
            });
            builder.Services.AddBusinessServices(builder.Configuration);

            // =========================================================================
            // BUILD
            // =========================================================================
            var app = builder.Build();

            var hostSetting = builder.Configuration["HostSettings:Host"] ?? "localhost";
            var portSetting = builder.Configuration["HostSettings:Port"] ?? "5249";

            // -------------------------------------------------------------------------
            // 5. MIDDLEWARE PIPELINE
            // -------------------------------------------------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // .NET 9 OPENAPI ENDPOINT (JSON Üretimi)
                // Bu satır /openapi/v1.json adresini oluşturur.
                app.MapOpenApi();

                // SCALAR UI (Arayüz)
                app.MapScalarApiReference(options =>
                {
                    // Scalar'ın yukarıdaki native openapi json'ını okumasını sağlıyoruz
                    options.WithOpenApiRoutePattern("/openapi/v1.json");
                    options.WithTitle("AEU OBS API");
                });

                // Tarayıcıyı Açma (Opsiyonel)
                try
                {
                    var url = $"http://{hostSetting}:{portSetting}/scalar/v1";
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                    {
                        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                    }
                }
                catch { }
            }

            app.ConfigureCustomExceptionMiddleware();

            app.UseCors();
            app.UseHttpsRedirection();

            // ÖNCE AuthN SONRA AuthZ
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run($"http://{hostSetting}:{portSetting}");
        }
    }
}