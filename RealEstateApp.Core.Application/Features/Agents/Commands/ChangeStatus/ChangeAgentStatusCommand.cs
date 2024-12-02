using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatus
{
    public class ChangeAgentStatusCommand : IRequest<Unit>
    {
        public string AgentId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ChangeAgentStatusCommandHandler : IRequestHandler<ChangeAgentStatusCommand, Unit>
    {
        private readonly IWebApiAccountService _accountService;

        public ChangeAgentStatusCommandHandler(IWebApiAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Unit> Handle(ChangeAgentStatusCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Recibido AgentId en ruta: {request.AgentId}");

            try
            {
                await _accountService.ChangeUserStatusAsync(request.AgentId, request.IsActive);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado del agente", ex);
            }
        }
    }
}
