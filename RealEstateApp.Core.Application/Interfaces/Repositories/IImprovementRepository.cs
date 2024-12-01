using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IImprovementRepository : IGenericRepository<Improvement>
    {
        Task<bool> HasAnyImprovementAsync();
        Task UpdateAsync(Improvement improvement);
    }
}
