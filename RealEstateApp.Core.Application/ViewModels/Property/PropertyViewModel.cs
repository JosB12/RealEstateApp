

namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyViewModel
    {
        public int Id { get; set; }
        public string PropertyCode { get; set; }
        public string PropertyType { get; set; }
        public string SaleType { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public double PropertySizeMeters { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Improvements { get; set; } = new List<string>();
        public string AgentName { get; set; }
        public string AgentPhoneNumber { get; set; }
        public string AgentPhotoUrl { get; set; }
        public string AgentEmail { get; set; }
        public bool IsFavorite { get; set; }

        
    }
}
