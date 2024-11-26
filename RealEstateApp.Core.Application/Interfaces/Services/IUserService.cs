using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.User;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<AuthenticationResponse> LogginAsync(LoginViewModel vm);
        Task SignOutAsync();
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "");
    }
}
