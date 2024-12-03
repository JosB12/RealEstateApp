using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Services
{
    public class DashboardAdminService : IDashboardAdminService
    {
        private readonly IUserService _userService;
        private readonly IPropertyRepository _propertyRepository;

        public DashboardAdminService (IUserService userService, IPropertyRepository propertyRepository)
        {
            _userService = userService;
            _propertyRepository = propertyRepository;
        }

        public async Task<int> GetTotalAgentAssetsAsync()
        {
            return await _userService.GetTotalAgentAssetsAsync();
        }

        public async Task<int> GetTotalAgentInactiveAsync()
        {
            return await _userService.GetTotalAgentInactiveAsync();
        }

        public async Task<int> GetTotalClientsAssetsAsync()
        {
            return await _userService.GetTotalClientsAssetsAsync();
        }

        public async Task<int> GetTotalClientsInactiveAsync()
        {
            return await _userService.GetTotalClientsInactiveAsync();
        }

        public async Task<int> GetTotalDeveloperAssetsAsync()
        {
            return await _userService.GetTotalDeveloperAssetsAsync();
        }

        public async Task<int> GetTotalDeveloperInactiveAsync()
        {
            return await _userService.GetTotalDeveloperInactiveAsync();
        }

        public async Task<int> GetTotalQuantityPropertyAvailableAsync()
        {
            return await _propertyRepository.GetTotalQuantityPropertyAvailableAsync();
        }

        public async Task<int> GetTotalQuantityPropertySoldAsync()
        {
            return await _propertyRepository.GetTotalQuantityPropertySoldAsync();
        }
    }
}
