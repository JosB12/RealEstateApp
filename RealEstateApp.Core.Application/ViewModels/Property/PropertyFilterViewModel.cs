
namespace RealEstateApp.Core.Application.ViewModels.Property
{
    public class PropertyFilterViewModel
    {
        public string PropertyCode { get; set; }
        public List<int> PropertyTypeIds { get; set; } = new List<int>();
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
    }
}
