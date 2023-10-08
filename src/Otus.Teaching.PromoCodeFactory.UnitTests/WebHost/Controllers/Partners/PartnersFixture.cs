using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnersFixture
    {
        public PartnersController partnersController { get; private set; }

        public PartnersFixture()
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();
            var serviceProvider = Configuration.GetServiceCollection(configuration)
                .ConfigureInMemoryContext()
                .BuildServiceProvider();

            partnersController = serviceProvider.GetService<PartnersController>();
        }
    }
}
