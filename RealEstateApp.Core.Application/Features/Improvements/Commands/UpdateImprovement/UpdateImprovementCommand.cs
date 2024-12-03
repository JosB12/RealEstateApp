
using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    public class UpdateImprovementCommand : IRequest<ImprovementUpdateResponse>
    {
        [SwaggerParameter(Description = "El id de la mejora que se esta actualizando")]
        public int? Id { get; set; }

        [SwaggerParameter(Description = "El nuevo nombre de la mejora")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una nueva descripcion para esta mejora")]
        public string? Description { get; set; }
    }
    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, ImprovementUpdateResponse>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<ImprovementUpdateResponse> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            // Validar que el ID esté presente
            if (!command.Id.HasValue)
                throw new ApiException("Improvement ID must be provided.", (int)HttpStatusCode.BadRequest);

            // Buscar la mejora en la base de datos
            var improvement = await _improvementRepository.GetByIdAsync(command.Id.Value);
            if (improvement == null)
                throw new ApiException($"Improvement with ID {command.Id} not found.", (int)HttpStatusCode.NotFound);

            // Actualizar solo los campos válidos
            if (!string.IsNullOrWhiteSpace(command.Name) && command.Name != "string")
                improvement.Name = command.Name;

            if (!string.IsNullOrWhiteSpace(command.Description) && command.Description != "string")
                improvement.Description = command.Description;

            // Guardar los cambios en la base de datos
            await _improvementRepository.UpdateAsync(improvement, improvement.Id);

            // Mapear la respuesta
            var improvementResponse = _mapper.Map<ImprovementUpdateResponse>(improvement);

            return improvementResponse;
        }


    }
}

