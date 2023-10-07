using AutoFixture;
using System;
using Moq;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Xunit;
using Otus.Teaching.PromoCodeFactory.WebHost.Controllers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests : IClassFixture<PartnersFixture>
    {
        PartnersController partnersController;

        public SetPartnerPromoCodeLimitAsyncTests(PartnersFixture fixture) 
        {
            partnersController = fixture.partnersController;
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_Partner_Not_Found_By_Guid()
        {
            var guid = Guid.NewGuid(); //random non-equivalent-to-existing guid

            var autoFixture = new Fixture();
            var request = autoFixture.Create<SetPartnerPromoCodeLimitRequest>();

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(guid, request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_Partner_Found_By_Guid_IsActive_false()
        {
            var autoFixture = new Fixture();
            //Evade recursion
            autoFixture.Customize<Partner>(x => x.With(par => par.PartnerLimits, new List<PartnerPromoCodeLimit>()));

            //Need custom controller with set up private repo in it
            var inactivePartner = autoFixture.Create<Partner>();
            inactivePartner.IsActive = false;
            var repoMock = new Mock<IRepository<Partner>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(inactivePartner);
            var customController = new PartnersController(repoMock.Object);

            var guid = Guid.NewGuid(); //random non-equivalent-to-existing guid

            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

            var result = await customController.SetPartnerPromoCodeLimitAsync(guid, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}