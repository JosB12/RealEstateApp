using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType
{
    public class CreatePropertyTypeCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El nombre del tipo de propiedad")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una descripcion del tipo de propiedad")]
        public string? Description { get; set; }

    }
    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeService, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeService;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = _mapper.Map<PropertyType>(command);
            propertyType = await _propertyTypeRepository.AddAsync(propertyType);
            return propertyType.Id;
        }

    }
}
