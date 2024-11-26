using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Controllers
{
    public class AgentController : Controller
    {

        private readonly IPropertyService _propertyService;
        private readonly AuthenticationResponse userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgentController(
            IHttpContextAccessor httpContextAccessor,
            IPropertyService propertyService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
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
    }
}
