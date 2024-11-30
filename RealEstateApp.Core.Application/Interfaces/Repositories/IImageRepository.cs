using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IImageRepository : IGenericRepository<Image>
    {
        Task<Image> GetByImageUrlAsync(string imageUrl);
    }
}
