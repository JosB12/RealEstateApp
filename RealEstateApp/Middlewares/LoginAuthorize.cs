using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealEstateApp.Controllers;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly ValidateUserSession _userSession;

        public LoginAuthorize(ValidateUserSession userSession)
        {
            _userSession = userSession;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificar si el usuario está autenticado
            if (_userSession.HasUser())
            {
                // Si el usuario ya está autenticado, no permitirle acceder al login
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                // Si no está autenticado, proceder con la acción
                await next();
            }
        }
    }

}
