using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{

    public class CreateImprovementCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "El nombre de la mejora")]
        public string? Name { get; set; }

        [SwaggerParameter(Description = "Una descripcion de la mejora")]
        public string? Description { get; set; }

    }
    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = _mapper.Map<Improvement>(command);
            improvement = await _improvementRepository.AddAsync(improvement);
            return improvement.Id;
        }

    }
}
