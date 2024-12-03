
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Commands.DeleteSaleTypeById
{
    public class DeleteSaleTypeByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteSaleTypeByIdCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, int>
    {
        private readonly ISalesTypeRepository _saleTypeRepository;
        public DeleteSaleTypeByIdCommandHandler(ISalesTypeRepository saleTypeRepository)
        {
            _saleTypeRepository = saleTypeRepository;
        }
        public async Task<int> Handle(DeleteSaleTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saleType == null) throw new ApiException($"Sale type not found.", (int)HttpStatusCode.BadRequest);

            await _saleTypeRepository.DeleteAsync(saleType);

            return saleType.Id;
        }
    }
}
