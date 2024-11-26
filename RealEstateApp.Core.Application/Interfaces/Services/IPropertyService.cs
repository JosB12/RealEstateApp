using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyAgentGeneralViewModel, Property>
    {
        Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId);
        Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
    }
}
