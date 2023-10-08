using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public static class Configuration
    {
        public static IServiceCollection GetServiceCollection(IConfigurationRoot configuration, IServiceCollection serviceCollection = null)
        {
            if (serviceCollection == null)
            {
                serviceCollection = new ServiceCollection();
            }
            serviceCollection
                .AddSingleton(configuration)
                .AddSingleton((IConfiguration)configuration)
                .AddMemoryCache();
            return serviceCollection;
        }

        public static IServiceCollection ConfigureInMemoryContext(this IServiceCollection services)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb", builder => { });
                options.UseInternalServiceProvider(serviceProvider);
            });
            services.AddTransient<DbContext, DataContext>();
            return services;
        }
    }
}
