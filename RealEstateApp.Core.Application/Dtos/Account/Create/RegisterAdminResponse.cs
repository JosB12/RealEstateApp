using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account.Create
{
    public class RegisterAdminResponse
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
