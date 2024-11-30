using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Services
{
    public class OfferService : GenericService<OfferSaveViewModel, OfferViewModel, Offer>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public OfferService(IOfferRepository offerRepository, IMapper mapper) : base(offerRepository, mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId)
        {
            var offers = await _offerRepository.GetAllAsync();
            var propertyOffers = offers.Where(o => o.PropertyId == propertyId).ToList();
            return _mapper.Map<List<OfferViewModel>>(propertyOffers);
        }
    }
}