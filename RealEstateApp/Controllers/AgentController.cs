using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Edit;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using System.Security.Claims;

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
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IOfferService _offerService;


        public AgentController(
            IHttpContextAccessor httpContextAccessor,
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            IImprovementService improvementService,
            ISalesTypeService salesTypeService,
            IPropertyRepository propertyRepository,
            IUserService userService, IChatService chatService, IOfferService offerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _improvementService = improvementService;
            _salesTypeService = salesTypeService;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _propertyRepository = propertyRepository;
            _userService = userService;
            _chatService = chatService;
            _offerService = offerService;

        }

        #region home
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

        #endregion

        #region Maintenance Properties
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
        #endregion

        #region create property
        //mostrar el formulario de creación de propiedad
        [HttpGet]
        public async Task<IActionResult> CreateProperty()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var propertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
            var saleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
            var improvements = await _improvementService.GetAllImprovementsNamesAsync();

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

        // Crear la propiedad
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

            if (model.Images.Count == 0)
            {
                model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
                model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
                model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

                model.HasError = true;
                model.Error = "Debe seleccionar al menos una imagen.";
                return View(model);
            }

            if (model.Images.Count > 4)
            {
                model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
                model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
                model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

                model.HasError = true;
                model.Error = "No puede seleccionar más de 4 imágenes.";
                return View(model);
            }

            var response = await _propertyService.CreatePropertyAsync(model);

            if (response.Success)
            {
                return RedirectToAction("PropertyMaintenance");
            }
            else
            {
                model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
                model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
                model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

                model.HasError = true;
                model.Error = response.Error;
                return View(model);
            }
        }


        #endregion

        #region delete property
        [HttpGet]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var property = await _propertyService.GetByIdForDeleteAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var property = await _propertyService.GetByIdForDeleteAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            var response = await _propertyService.DeletePropertyAsync(id);

            if (response)
            {
                return RedirectToAction("PropertyMaintenance");
            }

            ModelState.AddModelError("", "No se pudo eliminar la propiedad.");
            return View(property);
        }

        #endregion

        #region Edit Property
        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            // Obtén la propiedad para editar
            var editPropertyViewModel = await _propertyService.GetByIdForEditAsync(id);
            if (editPropertyViewModel == null)
            {
                return NotFound();
            }

            // Obtén los datos adicionales para los selects
            editPropertyViewModel.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
            editPropertyViewModel.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
            editPropertyViewModel.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

            // Verifica si faltan datos necesarios
            if (!editPropertyViewModel.PropertyTypes.Any() ||
                !editPropertyViewModel.SaleTypes.Any() ||
                !editPropertyViewModel.Improvements.Any())
            {
                ViewBag.ErrorMessage = "No hay tipos de propiedades, tipos de venta o mejoras disponibles para la edición.";
                return View("Error");
            }

            return View(editPropertyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(EditPropertyViewModel model)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var editPropertyViewModel = await _propertyService.GetByIdForEditAsync(model.Id);

            model.CurrentImageUrls = editPropertyViewModel.CurrentImageUrls;

            model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
            model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
            model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

            // Limpiar el error
            model.HasError = false;
            model.Error = string.Empty;

            // Si el modelo no es válido, recargar los datos de los selects
            if (!ModelState.IsValid)
            {
                model.CurrentImageUrls = editPropertyViewModel.CurrentImageUrls;
                model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
                model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
                model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

                return View(model);
            }

            // Llamar al servicio para actualizar la propiedad
            var result = await _propertyService.EditPropertyAsync(model);

            if (result.Success)
            {
                return RedirectToAction("PropertyMaintenance");
            }
            else
            {
                // Recargar los datos en caso de error
                model.CurrentImageUrls = editPropertyViewModel.CurrentImageUrls;
                model.PropertyTypes = await _propertyTypeService.GetAllPropertyTypesNameAsync();
                model.SaleTypes = await _salesTypeService.GetAllSaleTypesNamesAsync();
                model.Improvements = await _improvementService.GetAllImprovementsNamesAsync();

                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(model);
            }
        }
        #endregion


        #region My Profile (Edit profile)
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _userService.GetUserProfileToEditAsync(userId);
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileRequest request)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _userService.UpdateUserProfileAsync(userId, request);
                if (!response.HasError)
                {
                    TempData["ShowReloginModal"] = true;
                    return RedirectToAction("EditProfile");
                }
                ModelState.AddModelError("", response.Error);
            }
            return View(request);
        }
        #endregion


        #region chat
        public async Task<IActionResult> Details(int id)
        {
            // Recuperar la propiedad por su ID, junto con las ofertas relacionadas
            var property = await _propertyService.GetByIdSaveViewModel(id);

            if (property == null)
            {
                return NotFound();
            }

            // Recuperar las ofertas si no están en el modelo de la propiedad
            if (property.Offers == null)
            {
                property.Offers = await _offerService.GetOffersByPropertyIdAsync(id);
            }

            // Verificar si el usuario está autenticado y si es un Agente
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                ViewBag.Chats = new List<ChatMessageViewModel>();
                return View(property);
            }

            // Recuperar los chats asociados a la propiedad y al agente actual
            var chats = await _chatService.GetChatsByPropertyAndUserIdAsync(id, user.Id);
            ViewBag.Chats = chats ?? new List<ChatMessageViewModel>();

            return View(property);
        }


        [HttpPost]
        public async Task<IActionResult> SendMessageAsAgent(int propertyId, string message, string clientId)
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null || !user.Roles.Contains("Agent"))
            {
                return Json(new { success = false, message = "User not authenticated or not an agent" });
            }

            // Validar que el agente tenga acceso a la propiedad
            var property = await _propertyService.GetByIdSaveViewModel(propertyId);


            var chat = new ChatMessageViewModel
            {
                UserId = clientId, // El cliente al que se envía el mensaje
                PropertyId = propertyId,
                Message = message,
                IsAgent = true,
                SendDate = DateTime.Now
            };

            await _chatService.SendMessageAsync(chat);
            return Json(new { success = true });
        }

        #endregion



        #region Offers
        public async Task<IActionResult> GetOffers(int propertyId)
        {
            // Obtén las ofertas actualizadas desde la base de datos
            var offers = await _offerService.GetOffersByPropertyIdAsync(propertyId);

            // Asegúrate de que la vista reciba el modelo con las ofertas actualizadas
            return View(offers);
        }

        [HttpGet]
        
        public async Task<IActionResult> ClientOffers(int propertyId, string clientId)
        {
            var offers = await _offerService.GetOffersForClientAsync(propertyId, clientId);

            if (offers == null || !offers.Any())
            {
                return NotFound("No offers found for the specified property and client.");
            }

            // Convertir manualmente las ofertas a ViewModel
            var offerViewModels = offers.Select(offer => new OfferViewModel
            {
                Id = offer.Id,
                Amount = offer.Amount,
                CreateDate = offer.CreateDate,
                PropertyId = offer.PropertyId,
                // Aquí mapeas las demás propiedades necesarias
            }).ToList();

            ViewData["PropertyId"] = propertyId;

            return View(offerViewModels);
        }



        [HttpPost]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _offerService.GetOfferByIdAsync(offerId);

            // Aceptar la oferta
            await _offerService.AcceptOfferAsync(offer);

            // Cambiar el estado de la propiedad a "vendida"
            await _propertyService.MarkAsSoldAsync(offer.PropertyId);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RejectOffer(int offerId)
        {
            var offer = await _offerService.GetOfferByIdAsync(offerId);

            // Rechazar la oferta
            await _offerService.RejectOfferAsync(offer);

            return Json(new { success = true });
        }
        #endregion








    }
}
