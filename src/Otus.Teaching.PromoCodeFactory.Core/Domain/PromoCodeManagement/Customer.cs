using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        :BaseEntity
    {
        [MaxLength(120)]
        public string FirstName { get; set; }

        [MaxLength(120)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(120)]
        public string Email { get; set; }

        public List<PromoCode> PromoCodes { get; set; } = new List<PromoCode>();

        public List<CustomerPreference> CustomerPreferences { get; set; } = new List<CustomerPreference>();
    }
}