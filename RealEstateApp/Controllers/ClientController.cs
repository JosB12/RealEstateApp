using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IPropertyService _propertyService;

        public ClientController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // Acción para mostrar la lista de propiedades disponibles
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAvailablePropertiesAsync();
            return View(properties);
        }

        // Acción para filtrar las propiedades
        [HttpPost]
        public async Task<IActionResult> Filter(PropertyFilterViewModel filter)
        {
            var properties = await _propertyService.FilterPropertiesAsync(filter);
            return View("Index", properties);
        }

        // Acción para mostrar los detalles de una propiedad específica
        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyService.GetByIdSaveViewModel(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }
    }
}