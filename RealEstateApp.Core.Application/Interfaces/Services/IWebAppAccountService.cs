using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IWebAppAccountService : IBaseAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "");
        Task SignOutAsync();
    }
}
