﻿using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Entities
{
    public class Image : AuditableBaseEntity
    {
        public string ImageUrl { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
