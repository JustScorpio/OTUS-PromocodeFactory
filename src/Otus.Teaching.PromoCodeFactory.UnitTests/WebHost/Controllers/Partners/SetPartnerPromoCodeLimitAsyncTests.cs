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
            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

            var result = await partnersController.SetPartnerPromoCodeLimitAsync(guid, request);

            Assert.IsType<NotFoundResult>(result);
        }

        //[Fact]
        //public async void SetPartnerPromoCodeLimitAsync_Partner_IsActive_false()
        //{
        //    partnersController.;
        //    var guid = Guid.NewGuid(); //random non-equivalent-to-existing guid
        //    var autoFixture = new Fixture();
        //    var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

        //    var result = await partnersController.SetPartnerPromoCodeLimitAsync(guid, request);

        //    Assert.IsType<NotFoundResult>(result);
        //}
    }
}