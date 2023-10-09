using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Партнеры
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnersController
        : ControllerBase
    {
        private readonly IRepository<Partner> _partnersRepository;

        public PartnersController(IRepository<Partner> partnersRepository)
        {
            _partnersRepository = partnersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            var partners = await _partnersRepository.GetAllAsync();

            //This shit fell with misterious NullArgumentException
            //var response = partners.Select(x => new PartnerResponse()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
            //    IsActive = true,
            //    PartnerLimits = x.PartnerLimits
            //        .Select(y => new PartnerPromoCodeLimitResponse()
            //        {
            //            Id = y.Id,
            //            PartnerId = y.PartnerId,
            //            Limit = y.Limit,
            //            CreateDate = y.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
            //            EndDate = y.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
            //            CancelDate = y.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
            //        }).ToList()
            //});

            var response = new List<PartnerResponse>();
            foreach (var partner in partners)
            {
                var partnerResponse = new PartnerResponse();
                partnerResponse.Id = partner.Id;
                partnerResponse.Name = partner.Name;
                partnerResponse.NumberIssuedPromoCodes = partner.NumberIssuedPromoCodes;
                partnerResponse.IsActive = partner.IsActive;
                partnerResponse.PartnerLimits = new List<PartnerPromoCodeLimitResponse>();
                foreach (var promocodeResponse in partner.PartnerLimits)
                {
                    var limitResponse = new PartnerPromoCodeLimitResponse();
                    limitResponse.Id = promocodeResponse.Id;
                    limitResponse.PartnerId = promocodeResponse.PartnerId;
                    limitResponse.Limit = promocodeResponse.Limit;
                    limitResponse.CreateDate = promocodeResponse.CreateDate.ToString("dd.MM.yyyy hh:mm:ss");
                    limitResponse.EndDate = promocodeResponse.EndDate.ToString("dd.MM.yyyy hh:mm:ss");
                    limitResponse.CancelDate = promocodeResponse.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss");

                    partnerResponse.PartnerLimits.Add(limitResponse);
                }

                response.Add(partnerResponse);
            }

            return Ok(response);
        }
        
        [HttpGet("{id}/limits/{limitId}")]
        public async Task<ActionResult<PartnerPromoCodeLimit>> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();
            
            var limit = partner.PartnerLimits
                .FirstOrDefault(x => x.Id == limitId);

            var response = new PartnerPromoCodeLimitResponse()
            {
                Id = limit.Id,
                PartnerId = limit.PartnerId,
                Limit = limit.Limit,
                CreateDate = limit.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                EndDate = limit.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                CancelDate = limit.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
            };
            
            return Ok(response);
        }
        
        [HttpPost("{id}/limits")]
        public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(Guid id, SetPartnerPromoCodeLimitRequest request)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);

            if (partner == null)
                return NotFound();
            
            //Если партнер заблокирован, то нужно выдать исключение
            if (!partner.IsActive)
                return BadRequest("Данный партнер не активен");
            
            //Установка лимита партнеру
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x => 
                !x.CancelDate.HasValue);
            
            if (activeLimit != null)
            {
                //Если партнеру выставляется лимит, то мы 
                //должны обнулить количество промокодов, которые партнер выдал, если лимит закончился, 
                //то количество не обнуляется
                partner.NumberIssuedPromoCodes = 0;
                
                //При установке лимита нужно отключить предыдущий лимит
                activeLimit.CancelDate = DateTime.Now;
            }

            if (request.Limit <= 0)
                return BadRequest("Лимит должен быть больше 0");
            
            var newLimit = new PartnerPromoCodeLimit()
            {
                Limit = request.Limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = request.EndDate
            };
            
            partner.PartnerLimits.Add(newLimit);

            await _partnersRepository.UpdateAsync(partner);
            //await _partnersLimitsRepository.AddAsync(newLimit);
            //await _partnersLimitsRepository.UpdateAsync(activeLimit);

            return CreatedAtAction(nameof(GetPartnerLimitAsync), new {id = partner.Id, limitId = newLimit.Id}, null);
        }
        
        [HttpPost("{id}/canceledLimits")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            var partner = await _partnersRepository.GetByIdAsync(id);
            
            if (partner == null)
                return NotFound();
            
            //Если партнер заблокирован, то нужно выдать исключение
            if (!partner.IsActive)
                return BadRequest("Данный партнер не активен");
            
            //Отключение лимита
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x => 
                !x.CancelDate.HasValue);
            
            if (activeLimit != null)
            {
                activeLimit.CancelDate = DateTime.Now;
            }

            await _partnersRepository.UpdateAsync(partner);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> CreatePartnerAsync(CreatePartnerRequest request)
        {
            var partner = new Partner()
            {
                Name = request.Name,
                NumberIssuedPromoCodes = 0,
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            await _partnersRepository.AddAsync(partner);

            return CreatedAtAction(nameof(GetPartnersAsync), new { id = partner.Id }, null);
        }
    }
}