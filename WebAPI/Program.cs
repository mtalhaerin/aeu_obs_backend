using Business.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Concrete.EntityFramework.OzlukDals;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Diagnostics;

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

            // Register application services
            builder.Services.AddScoped<IKullaniciService, KullaniciManager>();
            builder.Services.AddScoped<IKullaniciDal, EFKullaniciDal>();

            var app = builder.Build();

            // Read host and port from appsettings.json
            var host = builder.Configuration["HostSettings:Host"] ?? "localhost";
            var port = builder.Configuration["HostSettings:Port"] ?? "5249";

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseReDoc(c =>
                {
                    c.SpecUrl("/openapi/v1.json");
                    c.RoutePrefix = "redoc"; // ReDoc at /redoc
                });
                app.MapScalarApiReference(); // Ensure Scalar API is mapped

                // Open the browser to the desired URL
                var url = $"http://{host}:{port}/scalar/";
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run($"http://{host}:{port}");
        }
    }
}
