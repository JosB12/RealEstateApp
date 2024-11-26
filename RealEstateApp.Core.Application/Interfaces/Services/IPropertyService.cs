using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
    }
}
