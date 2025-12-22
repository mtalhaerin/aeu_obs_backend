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
using Scalar.AspNetCore;
using System.Diagnostics;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------------------------------------------------
            // 1. AUTOFAC KURULUMU (Host seviyesinde yapılır)
            // -------------------------------------------------------------------------
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    // Eski kodundaki RegisterModule kısmı buraya geliyor
                    containerBuilder.RegisterModule(new AutofacBusinessModule());
                });

            // -------------------------------------------------------------------------
            // 2. SERVİS KAYITLARI (Services)
            // -------------------------------------------------------------------------
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

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
                            if (host == "localhost" || host == "127.0.0.1") return true;
                            if (host.EndsWith(".yourdomain.com") || host == "yourdomain.com") return true;
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

            // Standart Servisler (Autofac kullanıyorsan bunlara gerek kalmayabilir ama manuel eklediğin için bıraktım)
            builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
            builder.Services.AddScoped<IKullaniciDal, EFKullaniciDal>();

            // -------------------------------------------------------------------------
            // 3. JWT TOKEN VE AUTHENTICATION AYARLARI
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
            // 4. CORE MODULE DEPENDENCY RESOLVERS (ServiceTool için gerekli)
            // -------------------------------------------------------------------------
            builder.Services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule()
            });

            // =========================================================================
            // UYGULAMA İNŞASI (BUILD)
            // =========================================================================
            var app = builder.Build();

            // Host ve Port okuma
            var hostSetting = builder.Configuration["HostSettings:Host"] ?? "localhost";
            var portSetting = builder.Configuration["HostSettings:Port"] ?? "5249";

            // -------------------------------------------------------------------------
            // 5. MIDDLEWARE BORU HATTI (PIPELINE)
            // -------------------------------------------------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Eski koddaki exception page
                app.MapOpenApi();
                app.UseReDoc(c =>
                {
                    c.SpecUrl("/openapi/v1.json");
                    c.RoutePrefix = "redoc";
                });
                app.MapScalarApiReference();

                // Tarayıcıyı açma işlemi (Windows only)
                try
                {
                    var url = $"http://{hostSetting}:{portSetting}/scalar/";
                    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                }
                catch { /* Linux/Docker ortamında hata vermemesi için boş bırakılabilir */ }
            }

            // Custom Exception Middleware kullanıyorsan burayı açabilirsin
            // app.ConfigureCustomExceptionMiddleware();

            app.UseCors();

            app.UseHttpsRedirection();

            // app.UseRouting(); // .NET 6+ da MapControllers çağrıldığında otomatik eklenir, manuel yazmaya gerek yok ama yazarsan da hata vermez.

            // DİKKAT: Authentication Authorization'dan ÖNCE gelmelidir.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run($"http://{hostSetting}:{portSetting}");
        }
    }
}