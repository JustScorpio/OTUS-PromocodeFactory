using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        [MaxLength(120)]
        public string FirstName { get; set; }

        [MaxLength(120)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(120)]
        public string Email { get; set; }

        public Role Role { get; set; }

        public Guid? RoleId { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}