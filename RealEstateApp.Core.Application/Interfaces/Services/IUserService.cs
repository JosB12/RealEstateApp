using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Edit;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.User;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LogginAsync(LoginViewModel vm);
        Task SignOutAsync();

        Task<List<AgentListViewModel>> GetAllAgentForViewAsync();

        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> DeleteAgentWithProperiesAsync(string agentId);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "");
        Task<EditProfileResponse> UpdateUserProfileAsync(string userId, EditProfileRequest request);
        Task<EditProfileRequest> GetUserProfileToEditAsync(string userId);
    }
}
