using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Features.SalesTypes.Commands.CreateSaleType;
using RealEstateApp.Core.Application.Features.SalesTypes.Commands.DeleteSaleTypeById;
using RealEstateApp.Core.Application.Features.SalesTypes.Commands.UpdateSaleType;
using RealEstateApp.Core.Application.Features.SalesTypes.Queries.GetAllSalesTypes;
using RealEstateApp.Core.Application.Features.SalesTypes.Queries.GetAllSaleTypeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de tipo de venta")]
    public class SaleTypeController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
          Summary = "Listado de tipos de ventas",
          Description = "Obtiene todos los tipos de ventas creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var SalesTypes = await Mediator.Send(new GetAllSalesTypesQuery());

            if (SalesTypes == null || SalesTypes.Count <= 0)
            {
                return NoContent();
            }

            return Ok(SalesTypes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
            Summary = "tipo de venta por id",
            Description = "Obtiene un tipo de venta utilizando el id como filtro"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var SaleType = await Mediator.Send(new GetSaleTypeByIdQuery { Id = id });

            if (SaleType == null)
            {
                return NoContent();
            }

            return Ok(SaleType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
              Summary = "Creacion de tipo de venta",
              Description = "Recibe los parametros necesarios para crear un nuevo tipo de venta"
              )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateSaleTypeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
                  Summary = "Actualizacion de tipo de venta",
                  Description = "Recibe los parametros necesarios para modificar un tipo de venta existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SalesTypeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateSaleTypeCommand command)
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
             Summary = "Eliminar un tipo de venta",
             Description = "Recibe los parametros necesarios para eliminar un tipo de venta existente"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });
            return NoContent();
        }
    }
}