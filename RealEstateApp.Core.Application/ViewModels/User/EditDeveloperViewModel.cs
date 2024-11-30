using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.User
{
    public class EditDeveloperViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar el apellido del usuario")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario")]
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe colocar una cedula")]
        public string? Identification { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo")]
        public string? Email { get; set; }
    }
}
