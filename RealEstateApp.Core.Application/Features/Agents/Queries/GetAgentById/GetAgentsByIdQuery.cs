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
        private readonly IUserApiService _userApiService;

        public GetAgentsByIdQueryHandler(IUserApiService userApiService)
        {
            _userApiService = userApiService;

        }
        public async Task<AgentApiDto> Handle(GetAgentsByIdQuery request, CancellationToken cancellationToken)
        {
            var agent = await _userApiService.GetAgentByIdAsync(request.Id);

            if (agent == null)
            {
                throw new KeyNotFoundException("Agent not found");
            }

            return agent;
        }
    }
}
