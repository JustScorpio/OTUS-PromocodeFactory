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
using System.Linq;
using FluentAssertions;

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

            result.Should().BeOfType<NotFoundResult>();
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

            var guid = Guid.NewGuid(); //random guid

            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

            var result = await customController.SetPartnerPromoCodeLimitAsync(guid, request);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public async void SetPartnerPromoCodeLimitAsync_Set_NumberIssuedPromoCodes_Zero(int numberIssuedPromoCodes)
        {
            var autoFixture = new Fixture();
            autoFixture.Customize<Partner>(x => x.With(par => par.PartnerLimits, new List<PartnerPromoCodeLimit>()));
            var partner = autoFixture.Create<Partner>();
            autoFixture.Customize<PartnerPromoCodeLimit>(x => x.With(ppcl => ppcl.Partner, partner));
            var partnerLimit = autoFixture.Create<PartnerPromoCodeLimit>();
            partner.IsActive = true;
            partner.PartnerLimits.Add(partnerLimit);
            partner.NumberIssuedPromoCodes = numberIssuedPromoCodes;
            partnerLimit.CancelDate = null;

            //Need custom controller with set up private repo in it
            var repoMock = new Mock<IRepository<Partner>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner);
            var customController = new PartnersController(repoMock.Object);

            var guid = Guid.NewGuid(); //random guid

            autoFixture.Customize<SetPartnerPromoCodeLimitRequest>(x => x.With(req => req.Limit, 10));
            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

            await customController.SetPartnerPromoCodeLimitAsync(guid, request);

            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_Set_Limit_CancelDate()
        {
            var autoFixture = new Fixture();
            autoFixture.Customize<Partner>(x => x.With(par => par.PartnerLimits, new List<PartnerPromoCodeLimit>()));
            var partner = autoFixture.Create<Partner>();
            autoFixture.Customize<PartnerPromoCodeLimit>(x => x.With(ppcl => ppcl.Partner, partner));
            var partnerLimit = autoFixture.Create<PartnerPromoCodeLimit>();
            partner.IsActive = true;
            partner.PartnerLimits.Add(partnerLimit);
            partnerLimit.CancelDate = null;

            //Need custom controller with set up private repo in it
            var repoMock = new Mock<IRepository<Partner>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner);
            var customController = new PartnersController(repoMock.Object);

            var guid = Guid.NewGuid(); //random guid

            autoFixture.Customize<SetPartnerPromoCodeLimitRequest>(x => x.With(req => req.Limit, 10));
            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();

            await customController.SetPartnerPromoCodeLimitAsync(guid, request);

            partnerLimit.CancelDate.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async void SetPartnerPromoCodeLimitAsync_PartnerPromoCodeLimit_Limit_Is_Zero(int limit)
        {
            var autoFixture = new Fixture();
            autoFixture.Customize<Partner>(x => x.With(par => par.PartnerLimits, new List<PartnerPromoCodeLimit>()));
            var partner = autoFixture.Create<Partner>();
            autoFixture.Customize<PartnerPromoCodeLimit>(x => x.With(ppcl => ppcl.Partner, partner));
            var partnerLimit = autoFixture.Create<PartnerPromoCodeLimit>();
            partner.IsActive = true;
            partner.PartnerLimits.Add(partnerLimit);
            partnerLimit.CancelDate = null;

            //Need custom controller with set up private repo in it
            var repoMock = new Mock<IRepository<Partner>>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(partner);
            var customController = new PartnersController(repoMock.Object);

            var guid = Guid.NewGuid(); //random guid

            autoFixture.Customize<SetPartnerPromoCodeLimitRequest>(x => x.With(req => req.Limit, 10));
            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();
            request.Limit = limit;

            var result = await customController.SetPartnerPromoCodeLimitAsync(guid, request);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_EnsureLimit_Saved_In_DB()
        {
            var autoFixture = new Fixture();

            //Create partner in DB
            var partnerRequest = autoFixture.Create<CreatePartnerRequest>();
            var createResult = await partnersController.CreatePartnerAsync(partnerRequest);

            //Get created partner from DB
            var getPartnerResult = (await partnersController.GetPartnersAsync()).Result as OkObjectResult;
            var partnerResponse = (getPartnerResult.Value as List<PartnerResponse>).FirstOrDefault();
            var partner = new Partner()
            {
                Id = partnerResponse.Id,
                Name = partnerResponse.Name,
                NumberIssuedPromoCodes = partnerResponse.NumberIssuedPromoCodes,
                IsActive = partnerResponse.IsActive,
                PartnerLimits = partnerResponse.PartnerLimits
                    .Select(y => new PartnerPromoCodeLimit()
                    {
                        Id = y.Id,
                        PartnerId = y.PartnerId,
                        Limit = y.Limit,
                        CreateDate = DateTime.Parse(y.CreateDate),
                        EndDate = DateTime.Parse(y.EndDate),
                        CancelDate = DateTime.Parse(y.CancelDate),
                    }).ToList()
            };

            //Update partners limits
            var request = autoFixture.Build<SetPartnerPromoCodeLimitRequest>().Create();
            await partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            //Get partner once more
            var anotherGetPartnerResult = (await partnersController.GetPartnersAsync()).Result as OkObjectResult;
            var anotherPartnerResponse = (anotherGetPartnerResult.Value as List<PartnerResponse>).FirstOrDefault();

            //Check if limit exists
            anotherPartnerResponse.PartnerLimits.Should().NotBeEmpty();
        }
    }
}