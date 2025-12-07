using Business.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // Register DbContext (AEUContext has OnConfiguring, but register anyway)
            //builder.Services.AddDbContext<AEUContext>(options => { /* optional configuration */ });

            // Register application services so DI can resolve IKullaniciService and IKullaniciDal
            builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
            builder.Services.AddScoped<IKullaniciDal, EFKullaniciDal>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseReDoc(c =>
                {
                    c.SpecUrl("/openapi/v1.json");
                    c.RoutePrefix = "redoc"; // ReDoc at /redoc
                });
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
