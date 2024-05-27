using Cloud4Feed.Application.Repository;
using Cloud4Feed.Application.Repository.Contract;
using Cloud4Feed.Application.BackgroundTask.Startup;
using Cloud4Feed.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Cloud4Feed.Application.Validator;
using Cloud4Feed.Application.Service.Internal.Contract;
using Cloud4Feed.Application.Service.Internal;

namespace Cloud4Feed.Application.Registration
{
    public static class Registrar
    {
        public static IServiceCollection RegisterECommerce(this IServiceCollection services, string connectionString, bool autoMigrate)
        {
            services.AddDbContext<ECommerceContext>(
      options => options.UseSqlServer(connectionString, optionsBuilder => optionsBuilder.MigrationsAssembly("Cloud4Feed.Infrastructure")))
                .AddValidatorsFromAssemblyContaining<ProductValidator>()
                .AddCacheService()
                .AddRepositories()
                .AddStartupHost()
                ;

            if (autoMigrate)
                services.AddStartupTask<MigrationRunner>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                  .AddScoped<IProductRepository, ProductRepository>()
                  .AddScoped<ICategoryRepository, CategoryRepository>()
                  .AddScoped<IUserRepository, UserRepository>();
        }

        private static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            return services
                 .AddScoped<ICacheService, CacheService>()
                 .AddMemoryCache();
        }

        private static IServiceCollection AddStartupHost(this IServiceCollection services)
        {
           return services.AddHostedService<StartupRunnerHostedService>();
        }

        public static IServiceCollection AddStartupTask<TStartupTask>(this IServiceCollection services) where TStartupTask : class, IStartupTask
        {
            return services
                .AddScoped<TStartupTask>()
                .AddScoped<IStartupTask, TStartupTask>((IServiceProvider sp) => sp.GetRequiredService<TStartupTask>());
        }

    }
}
