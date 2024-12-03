using MediatR;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<IList<AgentApiDto>>  // Verifica que esta sea la firma correcta
    {
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IList<AgentApiDto>>  // Debe implementar IRequestHandler correctamente
    {
        private readonly IUserApiService _userApiService;

        public GetAllAgentsQueryHandler(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        public async Task<IList<AgentApiDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agentList = await _userApiService.GetAllAgentsForApiAsync();
            return agentList ?? new List<AgentApiDto>();  
        }
    }
}
