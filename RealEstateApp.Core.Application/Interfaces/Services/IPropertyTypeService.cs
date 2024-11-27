using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyTypeService : IGenericService<PropertyTypeViewModel, PropertyTypeViewModel, PropertyType>
    {
        Task<List<PropertyTypeViewModel>> GetAllPropertyTypesNameAsync();
        Task<List<PropertyTypeViewModel>> GetAllAsync();
    }
}
