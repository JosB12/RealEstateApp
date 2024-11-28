using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IDashboardAdminService
    {
        Task<int> GetTotalQuantityPropertyAvailableAsync();
        Task<int> GetTotalQuantityPropertySoldAsync();
        Task<int> GetTotalAgentAssetsAsync();
        Task<int> GetTotalAgentInactiveAsync();
        Task<int> GetTotalClientsAssetsAsync();
        Task<int> GetTotalClientsInactiveAsync();
        Task<int> GetTotalDeveloperAssetsAsync();
        Task<int> GetTotalDeveloperInactiveAsync();
    }
}
