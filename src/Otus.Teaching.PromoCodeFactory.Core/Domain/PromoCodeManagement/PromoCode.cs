using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(120)]
        public string Code { get; set; }

        [MaxLength(250)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(30);

        [MaxLength(120)]
        public string PartnerName { get; set; }

        public virtual Employee PartnerManager { get; set; }

        public Guid PartnerManagerId { get; set; }

        public virtual Preference Preference { get; set; }

        public Guid PreferenceId { get; set; }

        public virtual Customer Customer { get; set; }

        public Guid CustomerId { get; set; }
    }
}