using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Queries.GetAllPropertiesTypes
{
    public class GetAllPropertiesTypesQuery : IRequest<IList<SalesTypeDto>>
    {
    }
    public class GetAllPropertiesTypesQueryHandler : IRequestHandler<GetAllPropertiesTypesQuery, IList<SalesTypeDto>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertiesTypesQueryHandler(
            IPropertyTypeRepository propertyTypeRepository,
            IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<IList<SalesTypeDto>> Handle(GetAllPropertiesTypesQuery request, CancellationToken cancellationToken)
        {
            var propertyList = await _propertyTypeRepository.GetAllAsync();
            if (propertyList == null || propertyList.Count == 0) return null;
            var propertyDtoList = _mapper.Map<IList<SalesTypeDto>>(propertyList);
            return propertyDtoList;
        }
    }
}
