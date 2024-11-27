using Microsoft.EntityFrameworkCore.Metadata;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class DashboardAdminService : IDashboardAdminService
    {
        private readonly IWebAppAccountService _webbAppAccountService;
        private readonly IPropertyRepository _propertyRepository;

        public DashboardAdminService (IWebAppAccountService webbAppAccountService, IPropertyRepository propertyRepository)
        {
            _webbAppAccountService = webbAppAccountService;
            _propertyRepository = propertyRepository;
        }

        public async Task<int> GetTotalAgentAssetsAsync()
        {
            return await _webbAppAccountService.GetTotalAgentAssetsAsync();
        }

        public async Task<int> GetTotalAgentInactiveAsync()
        {
            return await _webbAppAccountService.GetTotalAgentInactiveAsync();
        }

        public async Task<int> GetTotalClientsAssetsAsync()
        {
            return await _webbAppAccountService.GetTotalClientsAssetsAsync();
        }

        public async Task<int> GetTotalClientsInactiveAsync()
        {
            return await _webbAppAccountService.GetTotalClientsInactiveAsync();
        }

        public async Task<int> GetTotalDeveloperAssetsAsync()
        {
            return await _webbAppAccountService.GetTotalDeveloperAssetsAsync();
        }

        public async Task<int> GetTotalDeveloperInactiveAsync()
        {
            return await _webbAppAccountService.GetTotalDeveloperInactiveAsync();
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
