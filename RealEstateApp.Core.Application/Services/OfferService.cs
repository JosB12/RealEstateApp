using AutoMapper;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;


namespace RealEstateApp.Core.Application.Services
{
    public class OfferService : GenericService<OfferSaveViewModel, OfferViewModel, Offer>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public OfferService(IOfferRepository offerRepository, IMapper mapper, IUserService userService) : base(offerRepository, mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Offer> GetOfferByIdAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null)
            {
                throw new Exception("Oferta no encontrada");
            }
            return offer;
        }


        public async Task<List<OfferViewModel>> GetOffersByPropertyIdAsync(int propertyId)
        {
            // Obtener todas las ofertas
            var offers = await _offerRepository.GetAllAsync();

            // Filtrar las ofertas por PropertyId
            var propertyOffers = offers.Where(o => o.PropertyId == propertyId).ToList();

            // Crear una lista para los ViewModels
            var offerViewModels = new List<OfferViewModel>();

            // Obtener los nombres de los usuarios asociados a cada oferta
            foreach (var offer in propertyOffers)
            {
                // Obtener el nombre del usuario
                var userName = await _userService.GetUserNameByIdAsync(offer.UserId);

                // Mapear la oferta al ViewModel
                var offerViewModel = _mapper.Map<OfferViewModel>(offer);

                // Asignar el nombre del usuario al ViewModel
                offerViewModel.ClientName = userName;

                // Agregar el ViewModel a la lista
                offerViewModels.Add(offerViewModel);
            }

            return offerViewModels;
        }


        public async Task<List<Offer>> GetOffersForClientAsync(int propertyId, string clientId)
        {
            return await _offerRepository.GetOffersByClientAndPropertyAsync(propertyId, clientId);
        }

        

        public async Task AcceptOfferAsync(Offer offer)
        {
            // Cambiar el estado de la oferta a "aceptada"
            offer.Status = OfferStatus.Accepted;
            await _offerRepository.UpdateAsync(offer, offer.Id);

            // Obtener todas las ofertas para la misma propiedad, pero solo rechazar las pendientes
            var offers = await _offerRepository.GetAllAsync();
            var pendingOffers = offers.Where(o => o.PropertyId == offer.PropertyId && o.Status == OfferStatus.Pending).ToList();

            // Rechazar todas las ofertas pendientes EXCEPTO la que acabamos de aceptar
            foreach (var pendingOffer in pendingOffers)
            {
                if (pendingOffer.Id != offer.Id) // Asegurarse de no rechazar la oferta que estamos aceptando
                {
                    pendingOffer.Status = OfferStatus.Rejected;
                    await _offerRepository.UpdateAsync(pendingOffer, pendingOffer.Id);
                }
            }
        }


        public async Task RejectOfferAsync(Offer offer)
        {
            // Cambiar el estado de la oferta a "rechazada"
            offer.Status = OfferStatus.Rejected;
            await _offerRepository.UpdateAsync(offer, offer.Id);
        }





    }
}