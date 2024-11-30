using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account.Get
{
    public class AgentDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfProperties { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
