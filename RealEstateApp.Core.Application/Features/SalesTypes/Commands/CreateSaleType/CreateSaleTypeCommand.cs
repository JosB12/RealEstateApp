
using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Commands.CreateSaleType
{
   
    public class CreateSaleTypeCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El nombre del tipo de venta")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una descripcion del tipo de venta")]
        public string? Description { get; set; }

    }
    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISalesTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public CreateSaleTypeCommandHandler(
            ISalesTypeRepository saleTypeRepository,
            IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = _mapper.Map<SaleType>(command);
            saleType = await _saleTypeRepository.AddAsync(saleType);
            return saleType.Id;
        }

    }
}
