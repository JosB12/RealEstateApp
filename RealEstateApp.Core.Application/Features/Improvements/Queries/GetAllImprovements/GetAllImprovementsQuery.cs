using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Improvement;
using RealEstateApp.Core.Application.Interfaces.Repositories;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements
{
    public class GetAllImprovementsQuery : IRequest<IList<ImprovementDto>>
    {
    }
    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, IList<ImprovementDto>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementsQueryHandler(
            IImprovementRepository improvementRepository,
            IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<IList<ImprovementDto>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
        {
            var improvementsList = await _improvementRepository.GetAllAsync();
            if (improvementsList == null || improvementsList.Count == 0) return null;
            var improvementsDtoList = _mapper.Map<IList<ImprovementDto>>(improvementsList);
            return improvementsDtoList;
        }
    }
}
