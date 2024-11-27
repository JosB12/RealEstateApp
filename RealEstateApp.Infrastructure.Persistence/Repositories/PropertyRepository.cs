using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationContext _dbContext;

        public PropertyRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId)
        {
            var properties = await _dbContext.Properties
                .Where(p => p.UserId == agentId && p.Status == (int)PropertyStatus.Available)
                .Include(p => p.Images)
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
                return new List<PropertyAgentGeneralViewModel>();
            }

            var propertyViewModels = properties.Select(p => new PropertyAgentGeneralViewModel
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
        public async Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _dbContext.Properties
                .Where(p => p.UserId == agentId &&
                      ((int)p.Status == (int)PropertyStatus.Available || p.Status == PropertyStatus.Sold))
                .Include(p => p.Images)
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
                return new List<HomeAgentPropertyViewModel>();
            }

            var propertyViewModels = properties.Select(p => new HomeAgentPropertyViewModel
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
        public async Task<int> GetTotalQuantityPropertyAvailableAsync()
        {
            return await _dbContext.Properties
                .CountAsync(p => p.Status == PropertyStatus.Available);
        }

        public async Task<int> GetTotalQuantityPropertySoldAsync()
        {
            return await _dbContext.Properties
                .CountAsync(p => p.Status == PropertyStatus.Sold);
        }
        public async Task<int> GetCountByAgentIdAsync(string agentId)
        {
            return await _dbContext.Properties.CountAsync(p => p.UserId == agentId);
        }
        public void DeleteProperties(List<Property> properties)
        {
            _dbContext.Properties.RemoveRange(properties);
            _dbContext.SaveChanges();
        }
        public async Task<List<Property>> GetPropertiesByAgentIdAsync(string agentId)
        {
            return await _dbContext.Properties
                                   .Where(p => p.UserId == agentId)  // Filtra propiedades por el ID del agente
                                   .ToListAsync();
        }

    }
}
