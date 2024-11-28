using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId);
        Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
        Task<int> GetTotalQuantityPropertyAvailableAsync();
        Task<int> GetTotalQuantityPropertySoldAsync();
        Task<int> GetCountByAgentIdAsync(string agentId);
        Task<List<Property>> GetPropertiesJustByAgentIdAsync(string agentId);
        void DeleteProperties(List<Property> properties);
        Task<Property> AddPropertyAsync(Property property);
        Task AddImagesAsync(List<Image> images);
        Task<string> GenerateUniquePropertyCodeAsync();
        Task DeleteAsync(Property property);

    }
}
