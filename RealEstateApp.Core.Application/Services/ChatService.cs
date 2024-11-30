using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class ChatService : GenericService<ChatMessageViewModel, ChatMessageViewModel, Chat>, IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;

        public ChatService(IChatRepository chatRepository, IMapper mapper) : base(chatRepository, mapper)
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
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
    }
}