using RealEstateApp.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account.Create
{
    public class RegisterDeveloperRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Identification { get; set; }

        public string Email { get; set; }

        public Roles UserRol { get; set; }
    }
}
