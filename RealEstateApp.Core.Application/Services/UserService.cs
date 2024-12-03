using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Create;
using RealEstateApp.Core.Application.Dtos.Account.Edit;
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

            if (vm.Photo != null && vm.Photo.Length > 0)
            {
                try
                {
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.Photo.FileName)}";

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagenes", "Usuarios");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string imagePath = Path.Combine(directoryPath, fileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await vm.Photo.CopyToAsync(stream);
                    }

                    registerRequest.PhotoUrl = $"/Imagenes/Usuarios/{fileName}";
                }
                catch (Exception ex)
                {
                    return new RegisterResponse
                    {
                        HasError = true,
                        Error = $"Error al subir la imagen: {ex.Message}"
                    };
                }
            }

            return await _accountService.RegisterBasicUserAsync(registerRequest, origin);
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

        #region Update(Edit) Profile

        public async Task<EditProfileRequest> GetUserProfileToEditAsync(string userId)
        {
            return await _accountService.GetUserByIdForEditiAsync(userId);
        }

        public async Task<EditProfileResponse> UpdateUserProfileAsync(string userId, EditProfileRequest request)
        {
            return await _accountService.UpdateUserAsync(userId, request);
        }
        #endregion


        #region Admin
        public async Task<List<AdminListViewModel>> GetAllAdminForViewAsync()
        {
            try
            {
                var admin = await _accountService.GetAllAdminsAsync();
                var adminList = _mapper.Map<List<AdminListViewModel>>(admin);
                return adminList;
            }
            catch (Exception ex)
            {
                // Manejar el error, como registrar el error y/o retornar un mensaje
                throw new ApplicationException("Error al obtener usuarios", ex);
            }
        }

        public async Task<RegisterAdminResponse> RegisterAdminAsync(SaveAdminViewModel vm)
        {
            var registerAdminRequest = _mapper.Map<RegisterAdminRequest>(vm);

            return await _accountService.CreateAdminAsync(registerAdminRequest);
        }

        public async Task<EditAdminViewModel> GetAdminForEditViewAsync(string userId)
        {
            var dto = await _accountService.GetAdminForEditAsync(userId);
            return _mapper.Map<EditAdminViewModel>(dto);
        }

        public async Task<UpdateUserResponse> EditAdminAsync(EditAdminViewModel vm, string loggedInUserId)
        {
            return await _accountService.UpdateAdminAsync(vm, loggedInUserId);

        }


        #endregion

        #region Developer
        public async Task<List<DeveloperListViewModel>> GetAllDeveloperForViewAsync()
        {
            try
            {
                var developer = await _accountService.GetlAllDeveloperAsync();
                var developerList = _mapper.Map<List<DeveloperListViewModel>>(developer);
                return developerList;
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Error al obtener usuarios", ex);
            }

        }

        public async Task<RegisterDeveloperResponse> RegisterDeveloperAsync(SaveDeveloperViewModel vm)
        {
            var registerDevloperRequest = _mapper.Map<RegisterDeveloperRequest>(vm);

            return await _accountService.CreateDeveloperAsync(registerDevloperRequest);
        }
        public async Task<EditDeveloperViewModel> GetDeveloperForEditViewAsync(string userId)
        {
            var dto = await _accountService.GetDeveloperForEditAsync(userId);
            return _mapper.Map<EditDeveloperViewModel>(dto);

        }
        public async Task<UpdateUserResponse> EditDeveloperAsync(EditDeveloperViewModel vm)
        {
            return await _accountService.UpdateDeveloperAsync(vm);

        }




        #endregion

    }
}
