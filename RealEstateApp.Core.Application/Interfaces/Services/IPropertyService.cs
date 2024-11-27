using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{

    public interface IPropertyService : IGenericService<PropertySaveViewModel, PropertyViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetAvailablePropertiesAsync();
        Task<List<PropertyViewModel>> FilterPropertiesAsync(PropertyFilterViewModel filter);
    }
}
