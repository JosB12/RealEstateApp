using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasAnyPropertyTypeAsync()
        {
            return await _dbContext.PropertyTypes.AnyAsync();
        }
        public async Task UpdateAsync(PropertyType propertyType)
        {
            _dbContext.PropertyTypes.Update(propertyType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetPropertyCountByTypeIdAsync(int typeId)
        {
            return await _dbContext.Properties.CountAsync(p => p.PropertyTypeId == typeId);
        }

        public async Task<PropertyType> GetByIdAsync(int id)
        {
            return await _dbContext.PropertyTypes
                .FirstOrDefaultAsync(pt => pt.Id == id);
        }
    }
}
