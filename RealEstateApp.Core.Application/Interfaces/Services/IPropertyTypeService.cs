using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeViewModel, PropertyTypeSaveViewModel, PropertyType>
    {
        Task<List<PropertyTypeViewModel>> GetAllPropertyTypesNameAsync();
        Task<List<PropertyTypeViewModel>> GetAllAsync();
        Task<PropertyTypeSaveViewModel> GetByIdAsync(int id);
        Task<int> GetPropertyCountByTypeIdAsync(int typeId);
        Task CreateAsync(PropertyTypeSaveViewModel propertyTypeSaveViewModel);
        Task EditAsync(int id, PropertyTypeSaveViewModel propertyTypeSaveViewModel);
        Task DeleteAsync(int id);
    }
}
