using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class SalesTypeRepository : GenericRepository<SaleType>, ISalesTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public SalesTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasAnySaleTypeAsync()
        {
            return await _dbContext.SaleTypes.AnyAsync();
        }
    }
}
