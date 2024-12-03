using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly ApplicationContext _dbContext;

        public ChatRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Chat>> GetChatsByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.Chats
                .Where(c => c.PropertyId == propertyId)
                .OrderBy(c => c.SendDate)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetChatsByPropertyAndUserIdAsync(int propertyId, string userId)
        {
            return await _dbContext.Chats
                .Where(c => c.PropertyId == propertyId && (c.UserId == userId || c.ReceiverId == userId))
                .OrderBy(c => c.SendDate)
                .ToListAsync();
        }

        public async Task<Chat> SendMessageAsync(Chat chat)
        {
            _dbContext.Chats.Add(chat);
            await _dbContext.SaveChangesAsync();
            return chat;
        }

        public async Task<List<Chat>> GetMessagesForAgentAsync(string agentId)
        {
            return await _dbContext.Chats
                .Where(c => c.ReceiverId == agentId)
                .OrderBy(c => c.SendDate)
                .ToListAsync();
        }

    }
}