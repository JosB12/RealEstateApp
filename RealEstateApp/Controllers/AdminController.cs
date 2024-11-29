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

        #region Agent

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

        #endregion

        #region Admin

        public async Task<IActionResult> AdminList()
        {
            try
            {
                // Obtener los agentes para la vista
                var admins = await _userService.GetAllAdminForViewAsync();
                return View(admins);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al obtener los agentes: " + ex.Message);
                return View(new List<AdminListViewModel>());
            }
        }
        [HttpPost]
        public async Task<IActionResult> DesactivateAdmin(string adminId)
        {
            if (string.IsNullOrEmpty(adminId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(AgentList));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.DeactivateUserAsync(adminId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario desactivado exitosamente";
            }

            return RedirectToAction(nameof(AdminList));
        }

        [HttpPost]
        public async Task<IActionResult> ActivateAdmin(string adminId)
        {
            if (string.IsNullOrEmpty(adminId))
            {
                TempData["Error"] = "ID de usuario no válido";
                return RedirectToAction(nameof(AgentList));
            }

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _userService.ActivateUserAsync(adminId, loggedInUserId);

            if (response.HasError)
            {
                TempData["Error"] = response.Error;
            }
            else
            {
                TempData["Success"] = "Usuario activado exitosamente";
            }

            return RedirectToAction(nameof(AdminList));
        }
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View(new SaveAdminViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(SaveAdminViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                // Llama al servicio para registrar un administrador
                var registerResponse = await _userService.RegisterAdminAsync(vm);

                if (registerResponse.HasError)
                {
                    ModelState.AddModelError("", registerResponse.Error);
                    return View(vm);
                }

                // Redirige al índice tras una creación exitosa
                return RedirectToAction(nameof(AdminList));
            }
            catch (Exception ex)
            {
                // Maneja errores y muestra detalles en la vista
                string errorDetails = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError("", "Error al crear el administrador: " + errorDetails);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAdmin(string adminId)
        {
            var user = await _userService.GetAdminForEditViewAsync(adminId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(EditAdminViewModel vm)
        {
            // Obtener el ID del usuario logueado
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verificar si el usuario logueado intenta editar su propia cuenta
            if (vm.Id == loggedInUserId)
            {
                TempData["Error"] = "El administrador logueado no puede editar su propia cuenta.";
                return RedirectToAction("AdminList");
            }

            var response = await _userService.EditAdminAsync(vm, loggedInUserId);

            if (response.HasError)
            {
                ModelState.AddModelError("", response.Error);
                return View(vm);
            }

            TempData["Success"] = "Administrador actualizado exitosamente.";
            return RedirectToAction("AdminList");
        }



        #endregion 


    }
}
