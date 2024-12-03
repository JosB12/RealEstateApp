using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Queries.GetAllPropertieTypeById
{
    public class GetPropertyTypeByIdQuery : IRequest<SalesTypeDto>
    {
        [SwaggerParameter(Description = "Debe colocar el id del tipo de propiedad del cual desea obtener la informacion")]
        [Required]
        public int Id { get; set; }
    }
    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, SalesTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(
            IPropertyTypeRepository propertyTypeRepository,
            IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;

        }

        public async Task<SalesTypeDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyTypeDto = await _propertyTypeRepository.GetByIdAsync(request.Id);
            var propertyTypeMap = _mapper.Map<SalesTypeDto>(propertyTypeDto);
            return propertyTypeMap;
        }
    }
}
