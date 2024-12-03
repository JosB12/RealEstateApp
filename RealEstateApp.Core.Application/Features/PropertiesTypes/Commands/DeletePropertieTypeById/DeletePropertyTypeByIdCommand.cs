using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.DeletePropertieTypeById
{
    public class DeletePropertyTypeByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El id del tipo de propiedad que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeletePropertyTypeByIdCommandHandler : IRequestHandler<DeletePropertyTypeByIdCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        public DeletePropertyTypeByIdCommandHandler(IPropertyTypeRepository propertyTypeService)
        {
            _propertyTypeRepository = propertyTypeService;
        }
        public async Task<int> Handle(DeletePropertyTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(command.Id);

            if (propertyType == null) throw new ApiException($"Property type not found.", (int)HttpStatusCode.BadRequest);

            await _propertyTypeRepository.DeleteAsync(propertyType);

            return propertyType.Id;
        }
    }
}
