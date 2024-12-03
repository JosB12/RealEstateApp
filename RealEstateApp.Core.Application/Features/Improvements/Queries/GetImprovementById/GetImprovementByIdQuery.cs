using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Improvement;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovementById
{
    public class GetImprovementByIdQuery : IRequest<ImprovementDto>
    {
        [SwaggerParameter(Description = "Debe colocar el id de la mejora la cual desea obtener la informacion")]
        [Required]
        public int Id { get; set; }
    }
    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, ImprovementDto>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<ImprovementDto> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            var improvementEntity = await _improvementRepository.GetByIdAsync(request.Id);
            if (improvementEntity == null)
            {
                throw new KeyNotFoundException($"Improvement with Id {request.Id} not found.");
            }
            var improvementsDtoMap = _mapper.Map<ImprovementDto>(improvementEntity);
            return improvementsDtoMap;
        }
    }

}
