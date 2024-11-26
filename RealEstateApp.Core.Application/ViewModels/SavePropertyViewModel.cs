using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class SavePropertyViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un tipo de propiedad")]
        public int PropertyTypeId { get; set; } // Tipo de propiedad (Apartamento, Casa, etc.)

        [Required(ErrorMessage = "Debe seleccionar un tipo de venta")]
        public int SaleTypeId { get; set; } // Tipo de venta (Alquiler, Venta, etc.)

        [Required(ErrorMessage = "Debe colocar un precio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; } // Precio de la propiedad

        [Required(ErrorMessage = "Debe colocar una descripción")]
        public string Description { get; set; } // Descripción de la propiedad

        [Required(ErrorMessage = "Debe colocar el tamaño de la propiedad")]
        [Range(1, double.MaxValue, ErrorMessage = "El tamaño debe ser mayor a 0.")]
        public double PropertySizeMeters { get; set; } // Tamaño de la propiedad en metros

        [Required(ErrorMessage = "Debe colocar la cantidad de habitaciones")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de habitaciones debe ser mayor a 0.")]
        public int Bedrooms { get; set; } // Número de habitaciones

        [Required(ErrorMessage = "Debe colocar la cantidad de baños")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de baños debe ser mayor a 0.")]
        public int Bathrooms { get; set; } // Número de baños

        [Required(ErrorMessage = "Debe seleccionar al menos una mejora")]
        public List<int> ImprovementIds { get; set; } // Lista de mejoras (puede ser un select múltiple)

        [Required(ErrorMessage = "Debe seleccionar al menos una imagen")]
        public List<IFormFile> Images { get; set; } // Lista de imágenes (máximo 4 imágenes)
    }
}
