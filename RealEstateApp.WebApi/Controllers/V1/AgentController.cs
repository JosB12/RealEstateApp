using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatus;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de agentes")]
    public class AgentController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
          Summary = "Listado de agentes",
          Description = "Obtiene todos los agentes creados"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentApiDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var Agents = await Mediator.Send(new GetAllAgentsQuery());

            if (Agents == null || Agents.Count <= 0)
            {
                return NoContent();
            }

            return Ok(Agents);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
             Summary = "agente por id",
             Description = "Obtiene un agente utilizando el id como filtro"
         )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentApiDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            var agent = await Mediator.Send(new GetAgentsByIdQuery { Id = id });

            if (agent == null)
            {
                return NoContent();
            }

            return Ok(agent);
        }

        [HttpGet("GetAgentProperty/{agentId}")]
        [Authorize(Roles = "Admin,Developer")]
        [SwaggerOperation(
            Summary = "propiedades de agente por id",
            Description = "Obtiene las propiedades del agente utilizando el id como filtro"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPropertyById(string agentId)
        {
            var query = new GetAgentPropertyQuery { AgentId = agentId };
            var properties = await Mediator.Send(query);

            if (properties == null || properties.Count == 0)
            {
                return NoContent();
            }

            return Ok(properties);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
         Summary = "Cambiar estado de un agente",
         Description = "Cambia el estado de un agente (activo o inactivo) utilizando su ID."
     )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeAgentStatusCommand command)
        {
            Console.WriteLine($"Recibido AgentId en ruta: {id}");
            Console.WriteLine($"AgentId en comando: {command?.AgentId}");

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("El ID del agente no puede estar vacío.");
            }

            if (command == null || string.IsNullOrEmpty(command.AgentId) || id != command.AgentId)
            {
                return BadRequest("El cuerpo de la solicitud es inválido o no coincide con el ID proporcionado.");
            }

            await Mediator.Send(command);

            return NoContent();
        }

    }
}
