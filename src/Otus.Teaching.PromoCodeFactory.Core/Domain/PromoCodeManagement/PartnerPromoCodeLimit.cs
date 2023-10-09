using System;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PartnerPromoCodeLimit : BaseEntity
    {
        public Guid PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public DateTime CreateDate { get; set; }

        public DateTime? CancelDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Limit { get; set; }
    }
}