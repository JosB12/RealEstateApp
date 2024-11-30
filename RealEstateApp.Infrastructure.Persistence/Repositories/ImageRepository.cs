
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;


namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ApplicationContext _dbContext;

        public ImageRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Image> GetByImageUrlAsync(string imageUrl)
        {
            return await _dbContext.Images
                .FirstOrDefaultAsync(img => img.ImageUrl == imageUrl);
        }


    }
}
