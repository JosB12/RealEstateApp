using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly IWebApiAccountService _accountApiService;

        private readonly IMapper _mapper;
        public UserApiService(IWebApiAccountService accountApiService, IMapper mapper)
        {
            _accountApiService = accountApiService;
            _mapper = mapper;
        }
        #region GetName
        public async Task<string> GetUserNameByIdAsync(string userId)
        {
           return await _accountApiService.GetUserNameByIdAsync(userId);

        }
        public async Task<AgentApiDto> GetAgentByIdAsync(string userId)
        {
            return await _accountApiService.GetAgentByIdAsync(userId);

        }
        public async Task<List<AgentApiDto>> GetAllAgentsForApiAsync()
        {
            return await _accountApiService.GetAllAgentsForApiAsync();
        }
        public async Task ChangeUserStatusAsync(string userId, bool isActive)
        {
            await _accountApiService.ChangeUserStatusAsync(userId, isActive);
        }
        #endregion
    }
}
