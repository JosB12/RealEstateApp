using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Property
{
    public class PropertyDto
    {
        public int Id { get; set; } 
        public string PropertyCode { get; set; }
        public string PropertyTypeName { get; set; } 
        public string SaleTypeName { get; set; } 
        public decimal Price { get; set; } 
        public double PropertySizeMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; } 
        public string Description { get; set; } 
        public List<string> Improvements { get; set; } 
        public string AgentName { get; set; } 
        public string AgentId { get; set; } 
        public string Status { get; set; } 
    }
}
