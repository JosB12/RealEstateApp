using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class SalesTypeService : GenericService<SaveSalesTypeViewModel, SaleTypeViewModel, SaleType>, ISalesTypeService
    {
        private readonly ISalesTypeRepository _salesTypeRepository;
        private readonly IMapper _mapper;
        public SalesTypeService(ISalesTypeRepository salesTypeRepository, IMapper mapper) : base(salesTypeRepository, mapper)
        {
            _salesTypeRepository = salesTypeRepository;
        }
        public async Task<List<SaleTypeViewModel>> GetAllSaleTypesNamesAsync()
        {
            var saleTypes = await _salesTypeRepository.GetAllAsync();
            return saleTypes.Select(s => new SaleTypeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        public async Task<List<SaleTypeViewModel>> GetAllAsync()
        {
            var saleTypes = await _salesTypeRepository.GetAllAsync();
            return saleTypes.Select(s => new SaleTypeViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                
            }).ToList();
        }

        public async Task<SaveSalesTypeViewModel> GetByIdAsync(int id)
        {
            var saleType = await _salesTypeRepository.GetByIdAsync(id);
            if (saleType == null)
            {
                // Opcional: Lanza una excepción controlada o devuelve null
                throw new NullReferenceException($"No se encontró un tipo de venta con el ID {id}.");
            }

            return new SaveSalesTypeViewModel
            {
                Id = saleType.Id,
                Name = saleType.Name,
                Description = saleType.Description
            };
        }

        public async Task<int> GetSalesTypeCountByIdAsync(int typeId)
        {
            return await _salesTypeRepository.GetSaleTypeCountByIdAsync(typeId);
        }

        public async Task CreateAsync(SaveSalesTypeViewModel salesTypeSaveViewModel)
        {
            if (salesTypeSaveViewModel == null)
            {
                throw new ArgumentNullException(nameof(salesTypeSaveViewModel), "El modelo no puede ser null");
            }

            // Asignación manual de propiedades
            var saleType = new SaleType
            {
                Name = salesTypeSaveViewModel.Name,
                Description = salesTypeSaveViewModel.Description
            };

            // Guardar en el repositorio
            await _salesTypeRepository.AddAsync(saleType);
        }



        public async Task EditAsync(int id, SaveSalesTypeViewModel salesTypeSaveViewModel)
        {
            var saleType = await _salesTypeRepository.GetByIdAsync(id);
            if (saleType != null)
            {
                saleType.Name = salesTypeSaveViewModel.Name;
                saleType.Description = salesTypeSaveViewModel.Description;
                await _salesTypeRepository.UpdateAsync(saleType);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var saleType = await _salesTypeRepository.GetByIdAsync(id);
            if (saleType != null)
            {
                // Eliminar todas las propiedades asociadas
                await _salesTypeRepository.DeleteAsync(saleType);
            }
        }
    }
}
