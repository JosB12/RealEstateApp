using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _dbContext.Properties
                .Where(p => p.UserId == agentId && p.Status == (int)PropertyStatus.Available)
                .Include(p => p.Images)  // Incluir las imágenes asociadas
                .Include(p => p.PropertyType)
                .Include(p => p.SaleType)
                .ToListAsync();

            Console.WriteLine($"Properties retrieved for agent {agentId}: {properties.Count}");
            if (!properties.Any())
            {
                Console.WriteLine("No properties found for this agent.");
            }

            if (properties == null)
            {
                return new List<PropertyViewModel>();
            }

            var propertyViewModels = properties.Select(p => new PropertyViewModel
            {
                PropertyCode = p.PropertyCode,
                PropertyType = p.PropertyType.Name, 
                SaleType = p.SaleType.Name, 
                Price = p.Price,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                PropertySizeMeters = p.PropertySizeMeters,
                Images = p.Images.Select(i => i.ImageUrl).ToList() 
            }).ToList();

            return propertyViewModels;
        }

    }
}
