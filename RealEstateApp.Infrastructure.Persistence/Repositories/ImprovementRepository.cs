using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ImprovementRepository : GenericRepository<Improvement>, IImprovementRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImprovementRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasAnyImprovementAsync()
        {
            return await _dbContext.Improvements.AnyAsync();
        }

        public async Task UpdateAsync(Improvement improvement)
        {
            _dbContext.Improvements.Update(improvement);
            await _dbContext.SaveChangesAsync();
        }
    }
}
