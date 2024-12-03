using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Edit;
using RealEstateApp.Core.Application.Dtos.Account.Create;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.User;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LogginAsync(LoginViewModel vm);
        Task SignOutAsync();
        Task<string> GetUserNameByIdAsync(string userId);

        Task<List<AgentListViewModel>> GetAllAgentForViewAsync();
        Task<List<AdminListViewModel>> GetAllAdminForViewAsync();
        Task<List<DeveloperListViewModel>> GetAllDeveloperForViewAsync();

        Task<RegisterAdminResponse> RegisterAdminAsync(SaveAdminViewModel vm);
        Task<RegisterDeveloperResponse> RegisterDeveloperAsync(SaveDeveloperViewModel vm);

        Task<EditAdminViewModel> GetAdminForEditViewAsync(string userId);
        Task<EditDeveloperViewModel> GetDeveloperForEditViewAsync(string userId);

        Task<UpdateUserResponse> EditAdminAsync(EditAdminViewModel vm, string loggedInUserId);
        Task<UpdateUserResponse> EditDeveloperAsync(EditDeveloperViewModel vm);

        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> DeleteAgentWithProperiesAsync(string agentId);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "");
        Task<EditProfileResponse> UpdateUserProfileAsync(string userId, EditProfileRequest request);
        Task<EditProfileRequest> GetUserProfileToEditAsync(string userId);
        Task<int> GetTotalAgentAssetsAsync();
        Task<int> GetTotalAgentInactiveAsync();
        Task<int> GetTotalClientsAssetsAsync();
        Task<int> GetTotalClientsInactiveAsync();
        Task<int> GetTotalDeveloperAssetsAsync();
        Task<int> GetTotalDeveloperInactiveAsync();
        Task<UserDto> GetUserByIdAsync(string UserId);
    }
    
}
