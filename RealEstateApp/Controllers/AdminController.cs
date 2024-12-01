using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.User;
using System.Security.Claims;

namespace RealEstateApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDashboardAdminService _dashboardAdminService;
        public readonly IUserService _userService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISalesTypeService _salesTypeService;
        private readonly IImprovementService _improvementService;


        public AdminController(IDashboardAdminService dashboardAdminService, IUserService userService, 
            IPropertyTypeService propertyTypeService,
            ISalesTypeService salesTypeService, IImprovementService improvementService)
        {
            _dashboardAdminService = dashboardAdminService;
            _userService = userService;
            _propertyTypeService = propertyTypeService;
            _salesTypeService = salesTypeService;
            _improvementService = improvementService;
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

        #region TiposPorpiedades
        [HttpGet]
        public async Task<IActionResult> PropertyTypesList()
        {
            var propertyTypes = await _propertyTypeService.GetAllAsync();

            // Asignar PropertyCount para cada tipo de propiedad
            foreach (var type in propertyTypes)
            {
                type.PropertyCount = await _propertyTypeService.GetPropertyCountByTypeIdAsync(type.Id);
            }

            return View(propertyTypes);
        }
        [HttpGet]
        public IActionResult CreatePropertyType()
        {
            return View(new PropertyTypeSaveViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> CreatePropertyType(PropertyTypeSaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _propertyTypeService.CreateAsync(model);
                return RedirectToAction("PropertyTypesList");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPropertyType(int id)
        {
            // Usar AutoMapper para mapear el PropertyType directamente a PropertyTypeSaveViewModel
            var propertyType = await _propertyTypeService.GetByIdAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }

            // AutoMapper mapea automáticamente gracias al perfil configurado
            return View(propertyType);  // Aquí propertyType ya es de tipo PropertyTypeSaveViewModel
        }

        [HttpPost]
        public async Task<IActionResult> EditPropertyType(int id, PropertyTypeSaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _propertyTypeService.EditAsync(id, model);
                TempData["Success"] = "Tipo de propiedad actualizado exitosamente.";
                return RedirectToAction("PropertyTypesList");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            var propertyType = await _propertyTypeService.GetByIdAsync(id);
            if (propertyType == null)
            {
                TempData["Error"] = "El tipo de propiedad no existe.";
                return RedirectToAction("PropertyTypesList");
            }

            return View(propertyType);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyTypes(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "El ID del tipo de propiedad no es válido.";
                return RedirectToAction("PropertyTypesList");
            }

             await _propertyTypeService.DeleteAsync(id);
            

            TempData["Success"] = "El tipo de propiedad se eliminó correctamente.";
            return RedirectToAction("PropertyTypesList");
        }

        #endregion

        #region TipoDeVentas
        [HttpGet]
        public async Task<IActionResult> SalesTypesList()
        {
            var salesTypes = await _salesTypeService.GetAllAsync();
            foreach (var type in salesTypes)
            {
                type.SaleTypeCount = await _salesTypeService.GetSalesTypeCountByIdAsync(type.Id);
            }


            return View(salesTypes);
        }

        [HttpGet]
        public IActionResult CreateSaleType()
        {
            return View(new SaveSalesTypeViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleType(SaveSalesTypeViewModel model)
        {
            if (model == null)
            {
                // Si el modelo es nulo, algo falló en el envío del formulario.
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await _salesTypeService.CreateAsync(model);
                return RedirectToAction("SalesTypesList");
            }

            // Manejar errores si el modelo no es válido
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditSaleType(int id)
        {
            var salesType = await _salesTypeService.GetByIdAsync(id);
            if (salesType == null)
            {
                return NotFound();
            }
            return View(salesType); // salesType debe ser de tipo SaveSalesTypeViewModel
        }


        [HttpPost]
        public async Task<IActionResult> EditSaleType(int id, SaveSalesTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _salesTypeService.EditAsync(id, model);
                TempData["Success"] = "Tipo de venta actualizado exitosamente.";
                return RedirectToAction("SalesTypesList");
            }
            TempData["Error"] = "Ocurrió un error al actualizar el tipo de venta.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSaleType(int id)
        {
            var salesType = await _salesTypeService.GetByIdAsync(id);
            if (salesType == null)
            {
                TempData["Error"] = "El tipo de venta no existe.";
                return RedirectToAction("SalesTypesList");
            }

            return View(salesType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSaleTypes(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "El ID del tipo de venta no es válido.";
                return RedirectToAction("SalesTypesList");
            }

            await _salesTypeService.DeleteAsync(id);


            TempData["Success"] = "El tipo de venta se eliminó correctamente.";
            return RedirectToAction("SalesTypesList");
        }




        #endregion

        #region Mejoras
        [HttpGet]
        public async Task<IActionResult> ImprovementList()
        {
            var improvements = await _improvementService.GetAllAsync();
            
            return View(improvements);
        }
        [HttpGet]
        public IActionResult CreateImprovement()
        {
            return View(new SaveImprovementViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> CreateImprovement(SaveImprovementViewModel model)
        {
            if (model == null)
            {
                // Si el modelo es nulo, algo falló en el envío del formulario.
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await _improvementService.CreateAsync(model);
                return RedirectToAction("ImprovementList");
            }

            // Manejar errores si el modelo no es válido
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditImprovement(int id)
        {
            var improvements = await _improvementService.GetByIdAsync(id);
            if (improvements == null)
            {
                return NotFound();
            }
            return View(improvements); 
        }


        [HttpPost]
        public async Task<IActionResult> EditImprovement(int id, SaveImprovementViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _improvementService.EditAsync(id, model);
                TempData["Success"] = "Mejora actualizada exitosamente.";
                return RedirectToAction("ImprovementList");
            }
            TempData["Error"] = "Ocurrió un error al actualizar la mejora.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImprovement(int id)
        {
            var improvements = await _improvementService.GetByIdAsync(id);
            if (improvements == null)
            {
                TempData["Error"] = "La mejora no existe.";
                return RedirectToAction("ImprovementList");
            }

            return View(improvements);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImprovements(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "El ID de la mejora no es válido.";
                return RedirectToAction("ImprovementList");
            }

            await _improvementService.DeleteAsync(id);


            TempData["Success"] = "La mejora se eliminó correctamente.";
            return RedirectToAction("ImprovementList");
        }

        #endregion


    }
}
