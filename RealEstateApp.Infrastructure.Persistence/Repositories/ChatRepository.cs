using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.ViewModels.ChatAgent;
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
        public async Task<List<string>> GetClientsByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.Chats
                .Where(c => c.PropertyId == propertyId && c.IsAgent == false)  // Filtramos solo mensajes de clientes
                .Select(c => c.UserId)  // Seleccionamos el UserId
                .Distinct()  // Eliminamos los duplicados
                .ToListAsync();  // Convertimos a lista
        }


        public async Task<List<ChatMessageAgentViewModel>> GetMessagesByClientAndPropertyAsync(string clientId, int propertyId)
        {
            return await _dbContext.Chats
                .Where(c => c.PropertyId == propertyId &&  // Filtramos por propiedad
                            (c.UserId == clientId || c.ReceiverId == clientId) &&  // Incluimos mensajes enviados o recibidos por el cliente
                            (c.IsAgent == true || c.IsAgent == false))  // Aseguramos que sea un mensaje de cliente o de agente
                .OrderBy(c => c.SendDate)  // Ordenamos por fecha de envío (para mostrar los mensajes en orden cronológico)
                .Select(c => new ChatMessageAgentViewModel
                {
                    SenderName = c.IsAgent ? "Agente" : c.UserId,  // Nombre del remitente, si es agente lo indicamos como "Agente"
                    Content = c.Message,  // Contenido del mensaje
                    DateSent = c.SendDate,  // Fecha en la que se envió el mensaje
                    SenderId = c.UserId,  // ID del remitente
                    ReceiverId = c.ReceiverId  // ID del receptor
                })
                .ToListAsync();  // Convertimos el resultado a una lista
        }

        public async Task SendMessageAsync(string senderId, string receiverId, int propertyId, string messageContent)
        {
            try
            {
                var chatMessage = new Chat
                {
                    UserId = senderId,  // El ID del agente que manda el mensaje
                    ReceiverId = receiverId,  // El ID del cliente
                    PropertyId = propertyId,  // El ID de la propiedad
                    Message = messageContent,  // El contenido del mensaje
                    IsAgent = true,  // Indicamos que es un mensaje de agente
                    SendDate = DateTime.UtcNow,  // Fecha de envío
                };

                // Guardamos el mensaje en la base de datos
                _dbContext.Chats.Add(chatMessage);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
                throw;
            }
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