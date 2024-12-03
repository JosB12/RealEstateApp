using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Favorite;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;

public class FavoriteService : GenericService<FavoriteSaveViewModel, FavoriteViewModel, Favorite>, IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public FavoriteService( 
        IFavoriteRepository favoriteRepository, 
        IPropertyRepository propertyRepository, 
        IMapper mapper,
        IUserService userService)
        : base(favoriteRepository, mapper)
    {
        _favoriteRepository = favoriteRepository;
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _userService = userService;

    }

    public async Task MarkAsFavoriteAsync(string userId, int propertyId)
    {
        var favorite = new Favorite
        {
            UserId = userId,
            PropertyId = propertyId,
            MarkedDate = DateTime.UtcNow
        };
        await _favoriteRepository.AddAsync(favorite);
    }

    public async Task UnmarkAsFavoriteAsync(string userId, int propertyId)
    {
        var favorite = await _favoriteRepository.GetByUserIdAndPropertyIdAsync(userId, propertyId);
        if (favorite != null)
        {
            await _favoriteRepository.DeleteINTAsync(favorite.Id);
        }
    }

    public async Task<bool> IsFavoriteAsync(string userId, int propertyId)
    {
        var favorite = await _favoriteRepository.GetByUserIdAndPropertyIdAsync(userId, propertyId);
        return favorite != null;
    }


    public async Task<List<PropertyViewModel>> GetFavoritePropertiesAsync(string userId)
    {
        var favorites = await _favoriteRepository.GetAllWithPropertiesAsync();
        var favoriteProperties = favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.Property)
            .Where(p => p != null) // Asegúrate de que la propiedad no sea null
            .ToList();
        var propertyViewModels = _mapper.Map<List<PropertyViewModel>>(favoriteProperties);
        foreach (var property in propertyViewModels)
        {
            var entity = favoriteProperties.FirstOrDefault(p => p.Id == property.Id);
            property.ImageUrl = entity?.Images?.FirstOrDefault()?.ImageUrl;
            property.Improvements = entity?.Improvements?.Select(i => i.Name).ToList() ?? new List<string>();
            var agent = await _userService.GetUserByIdAsync(entity.UserId);
            property.AgentName = agent?.FirstName + " " + agent?.LastName;
            property.AgentPhoneNumber = agent?.PhoneNumber;
            property.IsFavorite = true; // Marcar como favorito
        }

        return propertyViewModels;
    }
}