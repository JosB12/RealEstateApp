using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;
using System;


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
        


        public async Task UpdateAsync(SaleType saleType)
        {
            _dbContext.SaleTypes.Update(saleType);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> GetSaleTypeCountByIdAsync(int saleTypeId)
        {
            return await _dbContext.Properties.CountAsync(p => p.SaleTypeId == saleTypeId);

        }
        



    }
}
