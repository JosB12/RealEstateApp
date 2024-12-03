

using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.PropertyType
{
    public class PropertyTypeSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de tipo de propiedad es obligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La descripcion del tipo de propiedad es obligatorio")]
        public string Description { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
