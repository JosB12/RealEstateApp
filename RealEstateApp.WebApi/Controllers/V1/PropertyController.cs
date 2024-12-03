using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyByCode;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Developer")]
    [SwaggerTag("Obtencion de propiedades")]
    public class PropertyController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
           Summary = "Listado de propiedades",
           Description = "Obtiene todas las propiedades creadas y la cantidad de mejoras que estan asociado a la misma"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var Properties = await Mediator.Send(new GetAllPropertiesQuery());

            if (Properties == null || Properties.Count <= 0)
            {
                return NoContent();
            }

            return Ok(Properties);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
             Summary = "propiedad por id",
             Description = "Obtiene una propiedad utilizando el id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await Mediator.Send(new GetPropertyByIdQuery { Id = id });

            if (property == null)
            {
                return NoContent();
            }

            return Ok(property);
        }

        [HttpGet("ByCode/{propertyCode}")]
        [SwaggerOperation(
      Summary = "propiedad por codigo",
      Description = "Obtiene una propiedad utilizando el codigo como filtro"
  )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode(string propertyCode)
        {
            Console.WriteLine($"Recibido PropertyCode: {propertyCode}");

            var property = await Mediator.Send(new GetPropertyByCodeQuery { PropertyCode = propertyCode });
           
            if (property == null)
            {
                return NoContent();
            }
            return Ok(property);
        }
    }
}
