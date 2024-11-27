using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
