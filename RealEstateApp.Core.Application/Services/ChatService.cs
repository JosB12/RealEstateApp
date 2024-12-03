using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.ViewModels.ChatAgent;
using RealEstateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class ChatService : GenericService<ChatMessageViewModel, ChatMessageViewModel, Chat>, IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ChatService(IChatRepository chatRepository, IMapper mapper, IUserService userService) : base(chatRepository, mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<List<ChatMessageViewModel>> GetChatsByPropertyIdAsync(int propertyId)
        {
            var chats = await _chatRepository.GetChatsByPropertyIdAsync(propertyId);
            return _mapper.Map<List<ChatMessageViewModel>>(chats);
        }

        public async Task<List<ChatMessageViewModel>> GetChatsByPropertyAndUserIdAsync(int propertyId, string userId)
        {
            var chats = await _chatRepository.GetChatsByPropertyAndUserIdAsync(propertyId, userId);
            return _mapper.Map<List<ChatMessageViewModel>>(chats);
        }

        public async Task<ChatMessageViewModel> SendMessageAsync(ChatMessageViewModel chatMessage)
        {
            var chat = _mapper.Map<Chat>(chatMessage);
            var savedChat = await _chatRepository.SendMessageAsync(chat);
            return _mapper.Map<ChatMessageViewModel>(savedChat);
        }

        public async Task<List<ChatMessageViewModel>> GetMessagesForAgentAsync(string agentId)
        {
            var chats = await _chatRepository.GetMessagesForAgentAsync(agentId);
            return _mapper.Map<List<ChatMessageViewModel>>(chats);
        }


        public async Task<List<string>> GetClientsByPropertyIdAsync(int propertyId)
        {
           var clientIds = await _chatRepository.GetClientsByPropertyIdAsync(propertyId);
            return clientIds;
        }

        public async Task<List<ChatMessageAgentViewModel>> GetMessagesByClientAndPropertyAsync(string clientId, int propertyId)
        {
            // Obtener los mensajes sin nombres desde el repositorio
            var messages = await _chatRepository.GetMessagesByClientAndPropertyAsync(clientId, propertyId);

            // Crear una lista de mensajes con el nombre del usuario en lugar del UserId
            var chatMessages = new List<ChatMessageAgentViewModel>();

            foreach (var message in messages)
            {
                // Si es un agente, el nombre es "Agente", si es un cliente obtenemos el nombre real desde IUserService
                var senderName = message.SenderName == "Agente"
                    ? "Agente"
                    : await _userService.GetUserNameByIdAsync(message.SenderId);  // Obtener el nombre del cliente

                // Actualizamos el SenderName con el nombre del usuario (cliente o agente)
                message.SenderName = senderName;

                // Agregar el mensaje actualizado a la lista
                chatMessages.Add(message);
            }

            return chatMessages;
        }
        public async Task SendMessageAsync(string senderId, string receiverId, int propertyId, string messageContent)
        {
            await _chatRepository.SendMessageAsync(senderId, receiverId, propertyId, messageContent);
        }

    }
}