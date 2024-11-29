
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Middlewares;

namespace RealEstateApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IFavoriteService _favoriteService;
    private readonly IOfferService _offerService;


    public HomeController(ILogger<HomeController> logger, IUserService userService, 
        IPropertyService propertyService, IPropertyTypeService propertyTypeService, 
        IFavoriteService favoriteService, IOfferService offerService)
    {
        _logger = logger;
        _userService = userService;
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _favoriteService = favoriteService;
        _offerService = offerService;
    }
    public async Task<IActionResult> Index()
    {
        var user = HttpContext.Session.Get<AuthenticationResponse>("user");
        var properties = await _propertyService.GetAvailablePropertiesAsync();

        if (user != null && user.Roles.Contains("Client"))
        {
            var favoriteProperties = await _favoriteService.GetFavoritePropertiesAsync(user.Id);
            var favoritePropertyIds = favoriteProperties.Select(p => p.Id).ToHashSet();

            foreach (var property in properties)
            {
                property.IsFavorite = favoritePropertyIds.Contains(property.Id);
            }
        }

        ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();
        return View(properties);
    }

    [HttpPost]
    public async Task<IActionResult> Filter(PropertyFilterViewModel filter)
    {
        var properties = await _propertyService.FilterPropertiesAsync(filter);
        ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();
        return View("FilterResults", properties);
    }

    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetByIdSaveViewModel(id);
        if (property == null)
        {
            return NotFound();
        }
        return View(property);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int propertyId)
    {
        var user = HttpContext.Session.Get<AuthenticationResponse>("user");
        if (user != null && user.Roles.Contains("Client"))
        {
            var isFavorite = await _favoriteService.IsFavoriteAsync(user.Id, propertyId);
            if (isFavorite)
            {
                await _favoriteService.UnmarkAsFavoriteAsync(user.Id, propertyId);
            }
            else
            {
                await _favoriteService.MarkAsFavoriteAsync(user.Id, propertyId);
            }
            return Json(new { isFavorite = !isFavorite });
        }
        return Json(new { isFavorite = false });
    }
    [HttpPost]
    public async Task<IActionResult> CreateOffer(OfferSaveViewModel offer)
    {
        var user = HttpContext.Session.Get<AuthenticationResponse>("user");
        if (user == null || !user.Roles.Contains("Client"))
        {
            return Json(new { success = false, message = "User not authenticated or not a client" });
        }

        offer.UserId = user.Id; 

        
        ModelState.Clear();
        TryValidateModel(offer);

        if (ModelState.IsValid)
        {
            await _offerService.Add(offer);
            return Json(new { success = true });
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        return Json(new { success = false, message = "Invalid data", errors });
    }

    #region login
    [ServiceFilter(typeof(LoginAuthorize))]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ServiceFilter(typeof(LoginAuthorize))]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        AuthenticationResponse userVm = await _userService.LogginAsync(vm);
        if (userVm != null && userVm.HasError != true)
        {
            HttpContext.Session.Set<AuthenticationResponse>("user", userVm); // Guardar el usuario en la sesi�n
           
            // Redirigir seg�n el rol del usuario
            if (userVm.Roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Admin"); // Redirigir a Admin
            }
            else if (userVm.Roles.Contains("Client"))
            {
                HttpContext.Session.SetString("userId", userVm.Id);
                return RedirectToAction("Index", "Client"); // Redirigir a Client
            }
            else if (userVm.Roles.Contains("Agent"))
            {
                HttpContext.Session.SetString("userId", userVm.Id);
                return RedirectToAction("Index", "Agent"); // Redirigir a Agent
            }
            else
            {
                return RedirectToAction("AccessDenied"); // Si no tiene un rol v�lido, redirigir a acceso denegado
            }
        }
        else
        {
            vm.HasError = true; // Establecer que ha habido un error
            vm.Error = userVm.Error; // Asignar el mensaje de error
            return View(vm); // Mostrar la vista con el error
        }
    }


    #endregion

    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> LogOut()
    {
        await _userService.SignOutAsync();
        HttpContext.Session.Remove("user");
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }


}
