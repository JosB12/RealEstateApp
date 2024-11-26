using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Middlewares;

namespace RealEstateApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IPropertyService _propertyService;
    public HomeController(IPropertyService propertyService, ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        _propertyService = propertyService;

    }

    public IActionResult Index()
    {
        return View();
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

    #region AccessDenied
    public IActionResult AccessDenied()
    {
        return View();
    }
    #endregion

    #region (Register)

    [ServiceFilter(typeof(LoginAuthorize))]
    public IActionResult JoinApp()
    {
        return View(new SaveUserViewModel());
    }

    [ServiceFilter(typeof(LoginAuthorize))]
    [HttpPost]
    public async Task<IActionResult> JoinApp(SaveUserViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        try
        {
            var origin = Request.Headers["origin"];
            RegisterResponse response = await _userService.RegisterAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            if (vm.UserType == Roles.Client)
            {
                return RedirectToAction("ConfirmEmailInfo"); 
            }
        }

        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred trying to register the user. " + ex.Message);
            return View(vm);
        }
        return RedirectToRoute(new { controller = "Home", action = "Index" });

    }
    public IActionResult ConfirmEmailInfo()
    {
        return View();
    }

    [ServiceFilter(typeof(LoginAuthorize))]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        string response = await _userService.ConfirmEmailAsync(userId, token);
        return View("ConfirmEmail", response);
    }
    #endregion

    #region LogOut
    public async Task<IActionResult> LogOut()
    {
        await _userService.SignOutAsync();
        HttpContext.Session.Remove("user");
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }
    #endregion


    #region Agent
    [HttpGet]
    public async Task<IActionResult> Agents(string searchQuery = "")
    {
        var agents = await _userService.GetActiveAgentsAsync(searchQuery);
        return View(agents);  
    }

    [HttpGet]
    public async Task<IActionResult> AgentProperties(string agentId)
    {
        Console.WriteLine($"Fetching properties for agentId: {agentId}");

        var properties = await _propertyService.GetPropertiesAvailableByAgentIdAsync(agentId);
        if (properties == null || !properties.Any())
        {
            Console.WriteLine("No properties found for this agent.");
        }

        return View(properties);  // Devolvemos las propiedades a la vista
    }
    #endregion


}
