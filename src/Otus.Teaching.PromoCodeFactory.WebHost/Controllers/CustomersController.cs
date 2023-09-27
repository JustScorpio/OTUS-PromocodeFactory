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
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomersController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Получить данные о всех клиентах
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            var customersModelList = customers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                }).ToList();

            return customersModelList;
        }

        /// <summary>
        /// Получить данные клиента по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            var preferences = customer.CustomerPreferences;
            var employeeModel = new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PromoCodes = customer.PromoCodes.Select(x => new PromoCodeShortResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName
                }).ToList(),
                Preferences = preferences.Select(x => new PreferenceResponse()
                {
                    Id = x.PreferenceId,
                    Name = x.Preference.Name
                }).ToList()
            };

            return employeeModel;
        }
        
        /// <summary>
        /// Завести нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
            customer.CustomerPreferences = request.PreferenceIds.Select(x => new CustomerPreference() { Customer = customer, PreferenceId = x }).ToList();

            await _customerRepository.CreateAsync(customer);
            return Ok();
        }
        
        /// <summary>
        /// Изменить данные о клиенте
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.CustomerPreferences
                .Where(x => !request.PreferenceIds.Contains(x.PreferenceId))
                .ToList()
                .ForEach(x => customer.CustomerPreferences.Remove(x));
            request.PreferenceIds
                .Where(x => !customer.CustomerPreferences.Any(y => y.PreferenceId == x))
                .ToList()
                .ForEach(x => customer.CustomerPreferences.Add(new CustomerPreference() {Customer = customer, PreferenceId = x}));

            await _customerRepository.UpdateAsync(customer);
            return Ok();
        }
        
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            await _customerRepository.RemoveAsync(id);
            return Ok();
        }
    }
}