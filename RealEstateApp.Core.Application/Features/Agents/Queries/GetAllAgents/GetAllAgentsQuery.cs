using MediatR;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents
{
    public class GetAllAgentsQuery : IRequest<IList<AgentApiDto>>
    {
    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IList<AgentApiDto>>
    {
        private readonly IWebApiAccountService _accountService;

        public GetAllAgentsQueryHandler(IWebApiAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IList<AgentApiDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            var agentList = await _accountService.GetAllAgentsForApiAsync();
            return agentList ?? new List<AgentApiDto>();
        }
    }
}
