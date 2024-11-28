using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _context;

        public PropertyRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            return await _context.Properties
                                 .Include(p => p.PropertyType)
                                 .Include(p => p.SaleType)
                                 .Include(p => p.Images)
                                 .ToListAsync();
        }
    }
}