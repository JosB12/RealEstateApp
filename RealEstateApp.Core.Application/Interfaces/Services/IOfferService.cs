

using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IOfferService : IGenericService<OfferSaveViewModel, OfferViewModel, Offer>
    {
        Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId);
        Task<Offer> GetOfferByIdAsync(int offerId);
        Task<List<Offer>> GetOffersForClientAsync(int propertyId, string clientId);
        Task RespondToOfferAsync(int offerId, OfferStatus status);
        Task AcceptOfferAsync(Offer offer);
        Task RejectOfferAsync(Offer offer);
    }
}
