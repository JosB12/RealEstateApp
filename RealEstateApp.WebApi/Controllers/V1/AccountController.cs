using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.RegisterApi;
using RealEstateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Sistema de membresia")]
    public class AccountController : BaseApiController
    {
        private readonly IWebApiAccountService _accountService;

        public AccountController(IWebApiAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(
      Summary = "Login de usuario",
      Description = "Autentica un usuario en el sistema y le retorna un JWT"
      )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }


        [HttpPost("register/developer")]
        [SwaggerOperation(
         Summary = "Creacion de usuario Developer",
         Description = "Recibe los parametros necesarios para crear un usuario con el rol de developer"
     )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterDeveloperAsync(RegisterApiRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterDeveloperUserAsync(request, origin));
        }



        [Authorize (Roles ="Admin")]
        [HttpPost("register/admin")]
        [SwaggerOperation(
            Summary = "Creacion de usuario administrador",
            Description = "Recibe los parametros necesarios para crear un usuario con el rol administrador"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RegisterAdminAsync(RegisterApiRequest request)
        {
                var currentUser = HttpContext.User;
                return Ok(await _accountService.RegisterAdminUserAsync(request, currentUser));
        }

    }
}
