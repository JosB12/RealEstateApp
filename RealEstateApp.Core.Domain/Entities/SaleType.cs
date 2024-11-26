using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class SaleType : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Property> Properties { get; set; }

    }
}
