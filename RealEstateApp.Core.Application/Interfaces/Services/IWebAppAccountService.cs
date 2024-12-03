using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Dtos.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.Dtos.Account.Create;
using RealEstateApp.Core.Application.Dtos.Account.EditUsers;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Application.Dtos.Account.Edit;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IWebAppAccountService : IBaseAccountService
    {
        Task<int> GetTotalAgentAssetsAsync();
        Task<int> GetTotalAgentInactiveAsync();
        Task<int> GetTotalClientsAssetsAsync();
        Task<int> GetTotalClientsInactiveAsync();
        Task<int> GetTotalDeveloperAssetsAsync();
        Task<int> GetTotalDeveloperInactiveAsync();
        Task<List<AgentDto>> GetAllAgentsAsync();
        Task<List<AdminDto>> GetAllAdminsAsync();
        Task<List<DeveloperDto>> GetlAllDeveloperAsync();   

        Task<EditAdminDto> GetAdminForEditAsync(string adminId);
        Task<EditDeveloperDto> GetDeveloperForEditAsync(string developerId);

        Task<UpdateUserResponse> UpdateAdminAsync(EditAdminViewModel vm, string loggedInUserId);
        Task<UpdateUserResponse> UpdateDeveloperAsync(EditDeveloperViewModel vm);


        Task<RegisterAdminResponse> CreateAdminAsync(RegisterAdminRequest request);
        Task<RegisterDeveloperResponse> CreateDeveloperAsync(RegisterDeveloperRequest request);



        Task<EditProfileResponse> UpdateUserAsync(string userId, EditProfileRequest request);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);

        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> DeleteAgentAsync(string agentId);

        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "");
        Task SignOutAsync();
        Task<EditProfileRequest> GetUserByIdForEditiAsync(string userId);
        Task<UserDto> GetUserByIdAsync(string userId);
    }
}
