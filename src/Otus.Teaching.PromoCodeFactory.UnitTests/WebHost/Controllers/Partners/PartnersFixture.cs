using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Moq;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnersFixture
    {
        public PartnersController partnersController { get; private set; }

        public PartnersFixture()
        {
            Mock<IRepository<Partner>> partnerRepoMock = new Mock<IRepository<Partner>>();
            partnersController = new PartnersController(partnerRepoMock.Object);
        }
    }
}
