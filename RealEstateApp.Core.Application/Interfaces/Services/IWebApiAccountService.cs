
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.RegisterApi;
using RealEstateApp.Core.Application.Dtos.Agent;
using System.Security.Claims;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IWebApiAccountService : IBaseAccountService
    {
        Task<string> GetUserNameByIdAsync(string userId);
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<List<AgentApiDto>> GetAllAgentsForApiAsync();
        Task<AgentApiDto?> GetAgentByIdAsync(string id);
        Task<string> GetUserByIdAsync(string userId);
        Task ChangeUserStatusAsync(string userId, bool isActive);
        Task<RegisterResponse> RegisterAdminUserAsync(RegisterApiRequest request, ClaimsPrincipal currentUser);
        Task<RegisterResponse> RegisterDeveloperUserAsync(RegisterApiRequest request, string origin);


    }
}
