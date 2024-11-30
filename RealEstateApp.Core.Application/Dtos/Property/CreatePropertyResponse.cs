
namespace RealEstateApp.Core.Application.Dtos.Property
{
    public class CreatePropertyResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public int PropertyId { get; set; } 
    }
}
