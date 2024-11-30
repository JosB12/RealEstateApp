using RealEstateApp.Core.Application.ViewModels.Chat;
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
    }
}