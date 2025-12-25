using AutoMapper;
using Business.Behaviors;
using Business.ContextCarrier;
using Business.Features.CQRS.Auth.Login;
using Business.ValidationRules.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business.Extensions
{
    /// <summary>
    /// Contains extension methods for registering Buisness layer services into the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all Buisness layer services, validators, mappers, helpers, and utilities into the DI container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">Optional application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            AddMediatRWithPipelineBehaviors(services);
            AddFluentValidation(services);
            AddAutoMapper(services);
            AddCoreBusinessServices(services, configuration);

            return services;
        }

        /// <summary>
        /// Registers MediatR and related pipeline behaviors.
        /// </summary>
        private static void AddMediatRWithPipelineBehaviors(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        }

        /// <summary>
        /// Registers FluentValidation validators.
        /// </summary>
        private static void AddFluentValidation(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Example explicit validator registration
            services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        }

        /// <summary>
        /// Configures AutoMapper with profiles from the current assembly.
        /// </summary>
        private static void AddAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            mapperConfig.AssertConfigurationIsValid();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }

        /// <summary>
        /// Registers core Buisness services, helpers, utilities, JWT core services and custom Redis connections.
        /// </summary>
        private static void AddCoreBusinessServices(IServiceCollection services, IConfiguration? configuration)
        {
            // Caching & JWT
            services.AddScoped<IUserContext, UserContext>();
            //services.AddScoped<ICacheService, UnifeCacheService>();
            //services.AddScoped<ISessionJwtService, SessionJwtService>();
            //services.AddSingleton<IPasswordUtility, PasswordUtility>();
            //services.AddSingleton<IOTPUtilitiy, OTPUtilitiy>();

        }
    }
}
