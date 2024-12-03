using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.ChatAgent
{
    public class ChatMessageAgentViewModel
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public int PropertyId { get; set; }
        public bool IsAgent { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
    }

}
