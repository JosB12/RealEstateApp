using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
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
        #region login - SignOut
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
        #endregion


        #region register
        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);

            if (string.IsNullOrEmpty(vm.PhotoUrl))
            {
                return new RegisterResponse
                {
                    HasError = true,
                    Error = "La URL de la foto es obligatoria."
                };
            }

            registerRequest.PhotoUrl = vm.PhotoUrl;

            try
            {
                var response = await _accountService.RegisterBasicUserAsync(registerRequest, origin);
                if (response.HasError)
                {
                    return new RegisterResponse
                    {
                        HasError = true,
                        Error = response.Error 
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while trying to register the user: {ex.Message}";

                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }

                return new RegisterResponse
                {
                    HasError = true,
                    Error = errorMessage
                };
            }
        }


        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }
        #endregion


        #region Agent List
        public async Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "")
        {
            return await _accountService.GetActiveAgentsAsync(searchQuery);
        }
        #endregion



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
