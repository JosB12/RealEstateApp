using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Char
{
    public class SendMessageDto
    {
        public string ClientId { get; set; }
        public int PropertyId { get; set; }
        public string Content { get; set; }
    }
}
