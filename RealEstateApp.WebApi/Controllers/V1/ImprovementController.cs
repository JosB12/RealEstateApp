using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Improvement;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovementById;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
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
    [SwaggerTag("Mantenimiento de mejoras")]
    public class ImprovementController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
          Summary = "Listado de mejoras",
          Description = "Obtiene todas las mejoras creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var Inprovements = await Mediator.Send(new GetAllImprovementsQuery());

            if (Inprovements == null || Inprovements.Count <= 0)
            {
                return NoContent();
            }

            return Ok(Inprovements);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
            Summary = "mejora por id",
            Description = "Obtiene una mejora utilizando el id como filtro"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var Inprovement = await Mediator.Send(new GetImprovementByIdQuery { Id = id });

            if (Inprovement == null)
            {
                return NoContent();
            }

            return Ok(Inprovement);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
              Summary = "Creacion de mejoras",
              Description = "Recibe los parametros necesarios para crear una nueva mejora"
              )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateImprovementCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
                  Summary = "Actualizacion de mejoras",
                  Description = "Recibe los parametros necesarios para modificar una mejora existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateImprovementCommand command)
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
             Summary = "Eliminar una mejora",
             Description = "Recibe los parametros necesarios para eliminar una mejora existente"
       )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });
            return NoContent();
        }
    }
}
