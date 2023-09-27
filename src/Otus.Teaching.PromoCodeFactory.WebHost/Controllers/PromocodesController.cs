using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promocodeRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<CustomerPreference> _customerPreferenceRepository;

        public PromocodesController(IRepository<PromoCode> promocodeRepository,
            IRepository<Employee> employeeRepository,
            IRepository<Preference> preferenceRepository,
            IRepository<CustomerPreference> customerPreferenceRepository)
        {
            _promocodeRepository = promocodeRepository;
            _employeeRepository = employeeRepository;
            _preferenceRepository = preferenceRepository;
            _customerPreferenceRepository = customerPreferenceRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promocodeRepository.GetAllAsync();

            var promocodesModelList = promocodes.Select(x =>
                new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName
                }).ToList();

            return promocodesModelList;
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //FIXME: Could be much easier of instead of employee NAME request contain its ID
            var partner = (await _employeeRepository.GetAllAsync()).FirstOrDefault(x => x.FullName == request.PartnerName);
            if (partner == null)
                return NotFound();

            //FIXME: Could be much easier of instead of preference NAME request contain its ID
            var preference = (await _preferenceRepository.GetAllAsync()).FirstOrDefault(x => x.Name == request.Preference);
            if (preference == null)
            {
                preference = new Preference() { Name = request.Preference };
                await _preferenceRepository.CreateAsync(preference);
            }

            var customerPreferences = (await _customerPreferenceRepository.GetAllAsync()).Where(x => x.PreferenceId == preference.Id);

            var tasks = new List<Task>();
            var promocodes = new List<PromoCode>();
            foreach (var customerPreference in customerPreferences)
            {
                var promocode = new PromoCode()
                {
                    Code = request.PromoCode,
                    ServiceInfo = request.ServiceInfo,
                    Preference = preference,
                    PartnerManager = partner,
                    PartnerName = request.PartnerName,
                    CustomerId = customerPreference.CustomerId
                };

                promocodes.Add(promocode);
                tasks.Add(_promocodeRepository.CreateAsync(promocode));
            }

            await Task.WhenAll(tasks);
            return Ok();
        }
    }
}
