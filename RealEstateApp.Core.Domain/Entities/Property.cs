using RealEstateApp.Core.Domain.Common;
using RealEstateApp.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Property : AuditableBaseEntity
    {
        public string? UserId { get; set; }

        [Required]
        public PropertyStatus Status { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        
        [Required]
        public int SaleTypeId { get; set; }
        public SaleType SaleType { get; set; }  

        public string Description { get; set; }

        [Required]
        [MaxLength(6)]
        public string PropertyCode { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        [Required]
        public decimal Price { get; set; }
        public double PropertySizeMeters { get; set; }
        public virtual ICollection<Improvement> Improvements { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
