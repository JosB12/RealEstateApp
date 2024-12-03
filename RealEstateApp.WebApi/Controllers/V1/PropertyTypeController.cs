using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.DeletePropertieTypeById;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Queries.GetAllPropertiesTypes;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Queries.GetAllPropertieTypeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de tipo de propiedades")]
    public class PropertyTypeController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
          Summary = "Listado de tipos de propiedad",
          Description = "Obtiene todos los tipos de propiedad creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var PropertyTypes = await Mediator.Send(new GetAllPropertiesTypesQuery());

            if (PropertyTypes == null || PropertyTypes.Count <= 0)
            {
                return NoContent();
            }

            return Ok(PropertyTypes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
            Summary = "tipo de propiedad por id",
            Description = "Obtiene un tipo de propiedad utilizando el id como filtro"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var PropertyType = await Mediator.Send(new GetPropertyTypeByIdQuery { Id = id });

            if (PropertyType == null)
            {
                return NoContent();
            }

            return Ok(PropertyType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
              Summary = "Creacion de tipo de propiedad",
              Description = "Recibe los parametros necesarios para crear un nuevo tipo de propiedad"
              )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreatePropertyTypeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
                  Summary = "Actualizacion de tipo de propiedad",
                  Description = "Recibe los parametros necesarios para modificar un tipo de propiedad existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePropertyTypeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
             Summary = "Eliminar un tipo de propiedad",
             Description = "Recibe los parametros necesarios para eliminar un tipo de propiedad existente, al eliminar un tipo de propiedad " +
             "se eliminan las propiedades asociada con esta"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeletePropertyTypeByIdCommand { Id = id });
            return NoContent();
        }
    }
}
