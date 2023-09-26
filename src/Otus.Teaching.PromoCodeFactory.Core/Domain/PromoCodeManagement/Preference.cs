using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        :BaseEntity
    {
        [MaxLength(120)]
        public string Name { get; set; }

        public virtual List<CustomerPreference> CustomerPreferences { get; set; } = new List<CustomerPreference>();
    }
}