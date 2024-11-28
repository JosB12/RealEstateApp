using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IFavoriteService _favoriteService;

        public ClientController(IPropertyService propertyService, IFavoriteService favoriteService)
        {
            _propertyService = propertyService;
            _favoriteService = favoriteService;
        }

        // Acción para mostrar la lista de propiedades disponibles
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Favorites()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Client"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var favoriteProperties = await _favoriteService.GetFavoritePropertiesAsync(user.Id);
            return View(favoriteProperties);
        }


    }
}