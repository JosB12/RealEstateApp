using RealEstateApp.Core.Application.Dtos.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserApiService
    {
        Task<string> GetUserNameByIdAsync(string userId);
        Task<AgentApiDto> GetAgentByIdAsync(string userId);
        Task<List<AgentApiDto>> GetAllAgentsForApiAsync();
        Task ChangeUserStatusAsync(string userId, bool isActive);
    }
}
