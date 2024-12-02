using MediatR;
using RealEstateApp.Core.Application.Dtos.PropertyType;
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
        private readonly IPropertyTypeService _propertyTypeService;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        public async Task<SalesTypeDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var propertyTypeDto = await _propertyTypeService.GetByIdAsync(request.Id);
            return propertyTypeDto;
        }
    }
}
