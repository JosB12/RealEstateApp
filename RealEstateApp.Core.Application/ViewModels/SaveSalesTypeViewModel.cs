using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class SaveSalesTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de tipo de venta es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripcion del tipo de venta es obligatorio")]
        public string Description { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }

    }
}
