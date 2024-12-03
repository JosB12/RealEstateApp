using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
    {
        Task<bool> HasAnyPropertyTypeAsync();
        Task UpdateAsync(PropertyType propertyType);
        Task<int> GetPropertyCountByTypeIdAsync(int typeId);

    }
}
