using RealEstateApp.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class HomeAgentPropertyViewModel
    {
        public int Id { get; set; }
        public string PropertyCode { get; set; }
        public string PropertyType { get; set; }
        public string SaleType { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double PropertySizeMeters { get; set; }
        public List<string> Images { get; set; }
        public PropertyStatus Status { get; set; }
    }
}
