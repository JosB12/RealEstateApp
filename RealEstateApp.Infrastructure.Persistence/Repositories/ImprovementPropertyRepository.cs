using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ImprovementPropertyRepository : GenericRepository<ImprovementProperty>, IImprovementPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImprovementPropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override async Task DeleteAsync(ImprovementProperty improvementProperty)
        {
            _dbContext.ImprovementProperties.Remove(improvementProperty);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ImprovementProperty>> GetByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.ImprovementProperties
                                    .Where(ip => ip.PropertyId == propertyId)
                                    .ToListAsync();
        }
    }
}
