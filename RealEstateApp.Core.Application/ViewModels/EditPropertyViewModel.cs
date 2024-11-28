using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class EditPropertyViewModel
    {
        // Propiedades básicas
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de propiedad")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de venta")]
        public int SaleTypeId { get; set; }

        [Required(ErrorMessage = "Debe colocar un precio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Debe colocar una descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Debe colocar el tamaño de la propiedad")]
        [Range(1, double.MaxValue, ErrorMessage = "El tamaño debe ser mayor a 0.")]
        public double PropertySizeMeters { get; set; }

        [Required(ErrorMessage = "Debe colocar la cantidad de habitaciones")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de habitaciones debe ser mayor a 0.")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Debe colocar la cantidad de baños")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de baños debe ser mayor a 0.")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una mejora")]
        public List<int> SelectedImprovements { get; set; } = new List<int>();

        public List<string> CurrentImageUrls { get; set; } = new List<string>();

        public List<IFormFile>? NewImages { get; set; }

        public List<string> DeleteImages { get; set; } = new List<string>();

        public List<PropertyTypeViewModel> PropertyTypes { get; set; } = new List<PropertyTypeViewModel>();
        public List<SaleTypeViewModel> SaleTypes { get; set; } = new List<SaleTypeViewModel>();
        public List<ImprovementViewModel> Improvements { get; set; } = new List<ImprovementViewModel>();

        public bool HasError { get; set; } = false;
        public string Error { get; set; } = string.Empty;
    }
}
