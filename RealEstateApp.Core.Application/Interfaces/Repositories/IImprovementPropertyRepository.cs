using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IImprovementPropertyRepository : IGenericRepository<ImprovementProperty>
    {
        Task DeleteAsync(ImprovementProperty improvementProperty);
        Task<List<ImprovementProperty>> GetByPropertyIdAsync(int propertyId);
    }
}
