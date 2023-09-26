using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Role
        : BaseEntity
    {
        [MaxLength(120)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}