
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
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


    public HomeController(ILogger<HomeController> logger, IUserService userService, IPropertyService propertyService, IPropertyTypeService propertyTypeService)
    {
        _logger = logger;
        _userService = userService;
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;

    }
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetAvailablePropertiesAsync();
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
            HttpContext.Session.Set<AuthenticationResponse>("user", userVm); // Guardar el usuario en la sesión
           
            // Redirigir según el rol del usuario
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
                return RedirectToAction("AccessDenied"); // Si no tiene un rol válido, redirigir a acceso denegado
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


}
