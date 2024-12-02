
using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Improvement;
using RealEstateApp.Core.Application.Dtos.SaleType;
using RealEstateApp.Core.Application.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Features.SalesTypes.Queries.GetAllSalesTypes
{
    public class GetAllSalesTypesQuery : IRequest<IList<SaleTypeDto>>
    {
    }
    public class GetAllSalesTypesQueryHandler : IRequestHandler<GetAllSalesTypesQuery, IList<SaleTypeDto>>
    {
        private readonly ISalesTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSalesTypesQueryHandler(
           ISalesTypeRepository saleTypeRepository,
            IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<IList<SaleTypeDto>> Handle(GetAllSalesTypesQuery request, CancellationToken cancellationToken)
        {
            var salesTypesList = await _saleTypeRepository.GetAllAsync();
            if (salesTypesList == null || salesTypesList.Count == 0) return null;
            var salesTypesDtoList = _mapper.Map<IList<SaleTypeDto>>(salesTypesList);
            return salesTypesDtoList;
        }
    }
}
