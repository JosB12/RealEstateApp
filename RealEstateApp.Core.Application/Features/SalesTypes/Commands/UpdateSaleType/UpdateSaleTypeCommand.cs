
using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Commands.UpdateSaleType
{
    public class UpdateSaleTypeCommand : IRequest<SaleTypeUpdateResponse>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "El nuevo nombre del tipo de venta")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una nueva descripcion para esta mejora")]
        public string? Description { get; set; }
    }
    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, SaleTypeUpdateResponse>
    {
        private readonly ISalesTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISalesTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<SaleTypeUpdateResponse> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            // Verificar si el ID está presente
            if (!command.Id.HasValue)
                throw new ApiException("Sales type ID must be provided.", (int)HttpStatusCode.BadRequest);

            // Buscar el tipo de venta en la base de datos
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id.Value);
            if (saleType == null)
                throw new ApiException($"Sales type with ID {command.Id} not found.", (int)HttpStatusCode.NotFound);

            // Lógica para actualizar solo campos válidos
            if (!string.IsNullOrWhiteSpace(command.Name) && command.Name != "string")
                saleType.Name = command.Name;

            if (!string.IsNullOrWhiteSpace(command.Description) && command.Description != "string")
                saleType.Description = command.Description;

            // Actualizar la entidad en la base de datos
            await _saleTypeRepository.UpdateAsync(saleType, saleType.Id);

            // Mapear la respuesta
            var saleTypeResponse = _mapper.Map<SaleTypeUpdateResponse>(saleType);

            return saleTypeResponse;
        }


    }
}
