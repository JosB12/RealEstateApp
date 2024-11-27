using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.User;

namespace RealEstateApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IWebAppAccountService _accountService;
        private readonly IMapper _mapper;
        public UserService(IWebAppAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        

        public async Task<AuthenticationResponse> LogginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        #region Agent
        public async Task<List<AgentListViewModel>> GetAllAgentForViewAsync()
        {
            try
            {
                var agent = await _accountService.GetAllAgentsAsync();
                var agentList = _mapper.Map<List<AgentListViewModel>>(agent);
                return agentList;
            }
            catch (Exception ex)
            {
                // Manejar el error, como registrar el error y/o retornar un mensaje
                throw new ApplicationException("Error al obtener usuarios", ex);
            }
        }
        public async Task<UpdateUserResponse> DeleteAgentWithProperiesAsync(string agentId)
        {
            return await _accountService.DeleteAgentAsync(agentId);
        }
        #endregion

        #region activate/desactivate users
        public async Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId)
        {
            return await _accountService.DeactivateUserAsync(userId, loggedInUserId);
        }

        public async Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId)
        {
            return await _accountService.ActivateUserAsync(userId, loggedInUserId);
        }

        
        #endregion



    }
}
