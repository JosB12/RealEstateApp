using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType
{
    public class UpdatePropertyTypeCommand : IRequest<PropertyTypeUpdateResponse>
    {
        [SwaggerParameter(Description = "El id del tipo de propiedad que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "El nuevo nombre del tipo de propiedad")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una nueva descripcion para este tipo de propiedad")]
        public string? Description { get; set; }
    }
    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, PropertyTypeUpdateResponse>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeService, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeService;
            _mapper = mapper;
        }
        public async Task<PropertyTypeUpdateResponse> Handle(UpdatePropertyTypeCommand command, CancellationToken cancellationToken)
        {
            // Verificar si el ID está presente
            if (!command.Id.HasValue)
                throw new ApiException("The PropertyType Id is required.", (int)HttpStatusCode.BadRequest);

            // Buscar el tipo de propiedad en la base de datos
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id.Value);
            if (propertyType == null)
                throw new ApiException($"Property type with Id {command.Id} not found.", (int)HttpStatusCode.NotFound);

            // Lógica para actualizar solo campos válidos
            if (!string.IsNullOrWhiteSpace(command.Name) && command.Name != "string")
                propertyType.Name = command.Name;

            if (!string.IsNullOrWhiteSpace(command.Description) && command.Description != "string")
                propertyType.Description = command.Description;

            // Actualizar la entidad en la base de datos
            await _propertyTypeRepository.UpdateAsync(propertyType, propertyType.Id);

            // Mapear la respuesta
            var propertyTypeResponse = _mapper.Map<PropertyTypeUpdateResponse>(propertyType);

            return propertyTypeResponse;
        }


    }
}
