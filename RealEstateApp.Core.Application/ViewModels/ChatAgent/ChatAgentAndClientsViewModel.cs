using RealEstateApp.Core.Application.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.ChatAgent
{
    public class ChatAgentAndClientsViewModel
    {
        // Información del cliente
        public string ClientId { get; set; }
        public string? ClientName { get; set; }
        public int PropertyId { get; set; }
        public List<ChatMessageAgentViewModel> Messages { get; set; } = new List<ChatMessageAgentViewModel>();

        // Campos para enviar mensajes
        public string SenderId { get; set; }
        public string Content { get; set; }
        public bool IsAgent { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }

}
