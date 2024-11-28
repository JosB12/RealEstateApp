

using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IOfferService : IGenericService<OfferSaveViewModel, OfferViewModel, Offer>
    {
        Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId, string userId);
    }
}
