
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<Favorite> GetByUserIdAndPropertyIdAsync(string userId, int propertyId);
        Task<List<Favorite>> GetAllWithPropertiesAsync();

    }
}
