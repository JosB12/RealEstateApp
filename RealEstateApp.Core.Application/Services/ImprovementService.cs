using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<ImprovementViewModel, ImprovementViewModel, Improvement>, IImprovementService
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        public ImprovementService(IImprovementRepository improvementRepository, IMapper mapper) : base(improvementRepository, mapper)
        {
            _improvementRepository = improvementRepository;
        }
        public async Task<List<ImprovementViewModel>> GetAllImprovementsNamesAsync()
        {
            var improvements = await _improvementRepository.GetAllAsync();
            return improvements.Select(i => new ImprovementViewModel
            {
                Id = i.Id,
                Name = i.Name
            }).ToList();
        }
    }
}
