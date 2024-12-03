
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories.Generic;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class OfferRepository : GenericRepository<Offer>, IOfferRepository
    {
        private readonly ApplicationContext _dbContext;

        public OfferRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Offer>> GetOffersByClientAndPropertyAsync(int propertyId, string clientId)
        {
            return await _dbContext.Offers
                .Where(o => o.PropertyId == propertyId && o.UserId == clientId)
                .OrderBy(o => o.CreateDate)
                .ToListAsync();
        }
        public async Task RespondToOfferAsync(int offerId, OfferStatus responseStatus)
        {
            var offer = await _dbContext.Offers.FindAsync(offerId);
            if (offer == null) throw new Exception("Offer not found");

            if (responseStatus == OfferStatus.Accepted)
            {
                // Marcar la oferta como aceptada
                offer.Status = OfferStatus.Accepted;

                // Rechazar otras ofertas pendientes de la misma propiedad
                var otherOffers = await _dbContext.Offers
                    .Where(o => o.PropertyId == offer.PropertyId && o.Status == OfferStatus.Pending)
                    .ToListAsync();

                foreach (var otherOffer in otherOffers)
                {
                    otherOffer.Status = OfferStatus.Rejected;
                }

                // Cambiar el estado de la propiedad a Vendida
                var property = await _dbContext.Properties.FindAsync(offer.PropertyId);
                if (property != null)
                {
                    property.Status = PropertyStatus.Sold;
                }
            }
            else if (responseStatus == OfferStatus.Rejected)
            {
                offer.Status = OfferStatus.Rejected;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetPendingOffersByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.Offers
                .Where(o => o.PropertyId == propertyId && o.Status == OfferStatus.Pending)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        




    }
}
