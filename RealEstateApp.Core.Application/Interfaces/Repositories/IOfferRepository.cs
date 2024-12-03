using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;


namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IOfferRepository : IGenericRepository<Offer>
    {
        Task RespondToOfferAsync(int offerId, OfferStatus responseStatus);
        Task<List<Offer>> GetOffersByClientAndPropertyAsync(int propertyId, string clientId);
        Task<List<Offer>> GetPendingOffersByPropertyIdAsync(int propertyId);
        Task SaveAsync();
    }
}
