using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class ImprovementService : GenericService<ImprovementViewModel, SaveImprovementViewModel, Improvement>, IImprovementService
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

        public async Task<List<ImprovementViewModel>> GetAllAsync()
        {
            var improvement = await _improvementRepository.GetAllAsync();
            return improvement.Select(s => new ImprovementViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,

            }).ToList();
        }

        public async Task<SaveImprovementViewModel> GetByIdAsync(int id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement == null)
            {
                // Opcional: Lanza una excepción controlada o devuelve null
                throw new NullReferenceException($"No se encontró un tipo de venta con el ID {id}.");
            }

            return new SaveImprovementViewModel
            {
                Id = improvement.Id,
                Name = improvement.Name,
                Description = improvement.Description
            };
        }

       


        public async Task CreateAsync(SaveImprovementViewModel SaveImprovementViewModel)
        {
            if (SaveImprovementViewModel == null)
            {
                throw new ArgumentNullException(nameof(SaveImprovementViewModel), "El modelo no puede ser null");
            }

            // Asignación manual de propiedades
            var improvement = new Improvement
            {
                Name = SaveImprovementViewModel.Name,
                Description = SaveImprovementViewModel.Description
            };
            await _improvementRepository.AddAsync(improvement);
        }

        public async Task EditAsync(int id, SaveImprovementViewModel SaveImprovementViewModel)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement != null)
            {
                improvement.Name = SaveImprovementViewModel.Name;
                improvement.Description = SaveImprovementViewModel.Description;
                await _improvementRepository.UpdateAsync(improvement);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement != null)
            {
                await _improvementRepository.DeleteAsync(improvement);
            }
        }
    }
}
