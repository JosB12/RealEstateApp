using MediatR;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    public class GetAgentsByIdQuery : IRequest<AgentApiDto>
    {
        [SwaggerParameter(Description = "Debe colocar el id del Agente que quiere obtener")]
        [Required]
        public string Id { get; set; }
    }

    public class GetAgentsByIdQueryHandler : IRequestHandler<GetAgentsByIdQuery, AgentApiDto>
    {
        private readonly IWebApiAccountService _accountService;

        public GetAgentsByIdQueryHandler(IWebApiAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<AgentApiDto> Handle(GetAgentsByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await _accountService.GetAgentByIdAsync(request.Id);

            if (agent == null)
            {
                throw new KeyNotFoundException("Agent not found");
            }

            return agent;
        }
    }
}
