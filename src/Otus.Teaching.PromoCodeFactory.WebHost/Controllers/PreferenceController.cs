using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Категории предпочтений
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить данные всех категорий предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferenceModelList = preferences.Select(x => 
                new PreferenceResponse()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            return preferenceModelList;
        }
        
        /// <summary>
        /// Получить данные о категории предпочтений по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceByIdAsync(Guid id)
        {
            var preference = await _preferenceRepository.GetByIdAsync(id);

            if (preference == null)
                return NotFound();

            var preferenceModel = new PreferenceResponse()
            {
                Id = preference.Id,
                Name = preference.Name
            };

            return preferenceModel;
        }
    }
}