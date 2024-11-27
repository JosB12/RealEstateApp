using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Dtos.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);

        Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId);
        Task<UpdateUserResponse> DeleteAgentAsync(string agentId);

        Task SignOutAsync();
    }
}
