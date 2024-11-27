using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.User;
using System.Security.Claims;

namespace RealEstateApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardAdminService _dashboardAdminService;
        public readonly IUserService _userService;

        public AdminController(IDashboardAdminService dashboardAdminService, IUserService userService)
        {
            _dashboardAdminService = dashboardAdminService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var viewmodel = new DashBoardAdminViewModel
            {
                TotalPropertiesAvailable = await _dashboardAdminService.GetTotalQuantityPropertyAvailableAsync(),
                TotalPropertiesSold = await _dashboardAdminService.GetTotalQuantityPropertySoldAsync(),
                TotalAgentsActive = await _dashboardAdminService.GetTotalAgentAssetsAsync(),
                TotalAgentsInactive = await _dashboardAdminService.GetTotalAgentInactiveAsync(),
                TotalClientsActive = await _dashboardAdminService.GetTotalClientsAssetsAsync(),
                TotalClientsInactive = await _dashboardAdminService.GetTotalClientsInactiveAsync(),
                TotalDevelopersActive = await _dashboardAdminService.GetTotalDeveloperAssetsAsync(),
                TotalDevelopersInactive = await _dashboardAdminService.GetTotalDeveloperInactiveAsync(),
            };
            return View(viewmodel);

        }

        public async Task<IActionResult> AgentList()
        {
            try
            {
                // Obtener los agentes para la vista
                var agents = await _userService.GetAllAgentForViewAsync();
                return View(agents);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los agentes: " + ex.Message);
                return View(new List<AgentListViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Desactivate(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(AgentList));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.DeactivateUserAsync(agentId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario desactivado exitosamente";
            }

            return RedirectToAction(nameof(AgentList));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(AgentList));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.ActivateUserAsync(agentId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario activado exitosamente";
            }

            return RedirectToAction(nameof(AgentList));
        }

        public async Task<IActionResult> DeleteAgent(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                TempData["Error"] = "El ID del agente no es válido.";
                return RedirectToAction("Index"); 
            }

            var response = await _userService.DeleteAgentWithProperiesAsync(agentId); 
            if (response.HasError)
            {
                TempData["Error"] = response.Error;
                return RedirectToAction("Index"); 
            }

            TempData["Success"] = "El agente ha sido eliminado correctamente.";
            return RedirectToAction(nameof(AgentList)); 
        }


       
    }
}
