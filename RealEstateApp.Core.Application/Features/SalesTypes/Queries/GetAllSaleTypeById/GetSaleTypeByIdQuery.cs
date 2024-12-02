using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.SaleType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Queries.GetAllSaleTypeById
{
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeDto>
    {
        [SwaggerParameter(Description = "Debe colocar el id del tipo de propiedad del cual desea obtener la informacion")]
        [Required]
        public int Id { get; set; }
    }
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeDto>
    {
        private readonly ISalesTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;
        public GetSaleTypeByIdQueryHandler(ISalesTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<SaleTypeDto> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var SalesType = await _saleTypeRepository.GetByIdAsync(request.Id);
            if (SalesType == null)
            {
                throw new KeyNotFoundException($"Sale type with Id {request.Id} not found.");
            }
            var SalesTypeDto = _mapper.Map<SaleTypeDto>(SalesType);

            return SalesTypeDto;
        }
    }
}
