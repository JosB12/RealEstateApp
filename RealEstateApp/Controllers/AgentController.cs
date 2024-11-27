using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels;

namespace RealEstateApp.Controllers
{
    public class AgentController : Controller
    {

        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISalesTypeService _salesTypeService;
        private readonly IImprovementService _improvementService;
        private readonly AuthenticationResponse userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgentController(
            IHttpContextAccessor httpContextAccessor,
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            IImprovementService improvementService,
            ISalesTypeService salesTypeService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _improvementService = improvementService;
            _salesTypeService = salesTypeService;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            string agentId = user.Id;

            var properties = await _propertyService.GetPropertiesByAgentIdAsync(agentId);
            if (properties == null || !properties.Any())
            {
                Console.WriteLine("No properties found for this agent.");
            }

            Console.WriteLine($"Agent {user.UserName} has {properties.Count} properties.");

            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    Console.WriteLine($"Property: {property.PropertyCode}, Status: {property.Status}");
                }
            }

            ViewData["AgentName"] = $"{user.UserName}";

            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> PropertyMaintenance()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var properties = await _propertyService.GetPropertiesAvailableByAgentIdAsync(user.Id);
            return View(properties);
        }
        // Método para mostrar el formulario de creación de propiedad
        [HttpGet]
        public async Task<IActionResult> CreateProperty()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            // Obtener los tipos de propiedad, tipos de venta y mejoras para llenar los select
            var propertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
            var saleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
            var improvements = await _improvementService.GetAllImprovementsNamesAsync();

            // Verificar si no existen tipos de propiedad, tipos de venta o mejoras
            if (!propertyTypes.Any() || !saleTypes.Any() || !improvements.Any())
            {
                return View("Error", new { Message = "No hay tipos de propiedad, tipos de venta o mejoras disponibles." });
            }

            // Preparar el ViewModel con los datos
            var model = new SavePropertyViewModel
            {
                PropertyTypes = propertyTypes,
                SaleTypes = saleTypes,
                Improvements = improvements
            };

            return View(model);
        }

        // Método para crear la propiedad
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProperty(SavePropertyViewModel model)
        {

            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            model.UserId = user.Id;

          
            var response = await _propertyService.CreatePropertyAsync(model);

            if (response.Success)
            {
                return RedirectToAction("PropertyMaintenance");
            }
            else
            {
                model.HasError = true; 
                model.Error = response.Error; 
                return View(model);
            }
        }


    }
}
