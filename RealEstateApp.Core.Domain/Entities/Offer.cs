using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Offer : AuditableBaseEntity
    {
        public string? UserId { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public OfferStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
