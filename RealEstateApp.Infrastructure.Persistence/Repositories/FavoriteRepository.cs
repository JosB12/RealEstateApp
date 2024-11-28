
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        private readonly ApplicationContext _context;

        public FavoriteRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Favorite> GetByUserIdAndPropertyIdAsync(string userId, int propertyId)
        {
            return await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        public async Task<List<Favorite>> GetAllWithPropertiesAsync()
        {
            return await _context.Favorites
                                 .Include(f => f.Property)
                                 .ThenInclude(p => p.Images)
                                 .Include(f => f.Property)
                                 .ThenInclude(p => p.Improvements)
                                 .Include(f => f.Property)
                                 .ThenInclude(p => p.PropertyType)
                                 .Include(f => f.Property)
                                 .ThenInclude(p => p.SaleType)
                                 .ToListAsync();
        }
    }
}
