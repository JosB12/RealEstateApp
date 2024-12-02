using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Queries.GetAllPropertiesTypes
{
    public class GetAllPropertiesTypesQuery : IRequest<IList<SalesTypeDto>>
    {
    }
    public class GetAllPropertiesTypesQueryHandler : IRequestHandler<GetAllPropertiesTypesQuery, IList<SalesTypeDto>>
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public GetAllPropertiesTypesQueryHandler(IPropertyTypeService propertyTypeService,
            IMapper mapper           )
        {
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }
        public async Task<IList<SalesTypeDto>> Handle(GetAllPropertiesTypesQuery request, CancellationToken cancellationToken)
        {
            var propertyList = await _propertyTypeService.GetAllAsync();
            if (propertyList == null || propertyList.Count == 0) return null;
            var propertyDtoList = _mapper.Map<IList<SalesTypeDto>>(propertyList);
            return propertyDtoList;
        }
    }
}
