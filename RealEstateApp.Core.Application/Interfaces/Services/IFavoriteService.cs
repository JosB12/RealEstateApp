using RealEstateApp.Core.Application.ViewModels.Favorite;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IFavoriteService : IGenericService<FavoriteSaveViewModel, FavoriteViewModel, Favorite>
    {
        Task MarkAsFavoriteAsync(string userId, int propertyId);
        Task UnmarkAsFavoriteAsync(string userId, int propertyId);
        Task<bool> IsFavoriteAsync(string userId, int propertyId);
        Task<List<PropertyViewModel>> GetFavoritePropertiesAsync(string userId);
    }
}
