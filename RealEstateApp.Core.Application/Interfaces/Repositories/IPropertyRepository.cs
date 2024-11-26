using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId);
        Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
    }
}
