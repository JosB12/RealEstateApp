using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class SaveImprovementViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la mejora es obligatoria")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descripcion de la mejora es obligatoria")]
        public string Description { get; set; }
    }
}
