using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Dtos.Account.Edit
{
    public class EditProfileRequest
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]
        public string Phone { get; set; }

        public IFormFile? Photo { get; set; }

       
    }
}
