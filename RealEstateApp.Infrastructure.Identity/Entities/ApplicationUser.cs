﻿    using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Photo { get; set; }
        public string Identification { get; set; }
        public bool IsActive { get; set; } = false;
        public Roles UserType { get; set; }
    }
}
