using RealEstateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        Task<List<Chat>> GetChatsByPropertyIdAsync(int propertyId);
        Task<List<Chat>> GetChatsByPropertyAndUserIdAsync(int propertyId, string userId);
        Task<Chat> SendMessageAsync(Chat chat);
        Task<List<Chat>> GetMessagesForAgentAsync(string agentId);
    }
}