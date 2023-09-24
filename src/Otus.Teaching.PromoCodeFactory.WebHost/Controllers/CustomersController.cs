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
        private readonly IRepository<CustomerPreference> _customerPreferencesRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<CustomerPreference> customerPreferencesRepository)
        {
            _customerRepository = customerRepository;
            _customerPreferencesRepository = customerPreferencesRepository;
        }

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
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

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
                Preferences = customer.CustomerPreferences.Select(x => new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Preference.Name
                }).ToList()
            };

            return employeeModel;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            await _customerRepository.CreateAsync(customer);

            var tasks = new List<Task>();
            foreach (var preferenceId in request.PreferenceIds)
            {
                var customerPreference = new CustomerPreference();
                customerPreference.Customer = customer;
                customerPreference.PreferenceId = preferenceId;
                //FIXME: new Save after each new customerPreference with current EfRepository implementation
                tasks.Add(_customerPreferencesRepository.CreateAsync(customerPreference));
            }
            await Task.WhenAll(tasks);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var tasks = new List<Task>();

            var customer = new Customer()
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
            };
            await _customerRepository.UpdateAsync(customer);


            var existingPreference = (await _customerPreferencesRepository.GetAllAsync())
                .Where(x => Equals(x.CustomerId, id));

            //Remove absent preferences
            foreach (var preference in existingPreference.Where(x => !request.PreferenceIds.Contains(x.PreferenceId)))
                tasks.Add(_customerPreferencesRepository.RemoveAsync(preference.Id));
            //Add new preferences
            foreach (var preferenceId in request.PreferenceIds.Where(x => !existingPreference.Any(y => Equals(y.PreferenceId, x))))
            {
                var customerPreference = new CustomerPreference()
                {
                    Customer = customer,
                    PreferenceId = preferenceId
                };
                tasks.Add(_customerPreferencesRepository.CreateAsync(customerPreference));
            }

            await Task.WhenAll(tasks);

            return CreatedAtAction(nameof(GetCustomerAsync), new { id = customer.Id }, customer);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            var preferences = customer.CustomerPreferences;

            var tasks = new List<Task>();

            foreach (var preference in preferences)
                tasks.Add(_customerPreferencesRepository.RemoveAsync(preference.Id));

            await Task.WhenAll(tasks);

            return Ok(_customerRepository.RemoveAsync(id));
        }
    }
}