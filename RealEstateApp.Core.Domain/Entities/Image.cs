using RealEstateApp.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Image : AuditableBaseEntity
    {
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
