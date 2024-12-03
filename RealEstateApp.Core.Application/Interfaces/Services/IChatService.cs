using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.ViewModels.ChatAgent;
using RealEstateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IChatService : IGenericService<ChatMessageViewModel, ChatMessageViewModel, Chat>
    {
        Task<List<ChatMessageViewModel>> GetChatsByPropertyIdAsync(int propertyId);
        Task<List<ChatMessageViewModel>> GetChatsByPropertyAndUserIdAsync(int propertyId, string userId);
        Task<ChatMessageViewModel> SendMessageAsync(ChatMessageViewModel chatMessage);
        Task<List<ChatMessageViewModel>> GetMessagesForAgentAsync(string agentId);
        Task<List<string>> GetClientsByPropertyIdAsync(int propertyId);
        Task<List<ChatMessageAgentViewModel>> GetMessagesByClientAndPropertyAsync(string clientId, int propertyId);
        Task SendMessageAsync(string senderId, string receiverId, int propertyId, string messageContent);
    }
}