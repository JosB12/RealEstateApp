using AutoMapper;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyTypeViewModel, PropertyTypeSaveViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;
        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper) : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyTypeViewModel>> GetAllPropertyTypesNameAsync()
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            return propertyTypes.Select(p => new PropertyTypeViewModel
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
        public async Task<List<PropertyTypeViewModel>> GetAllAsync()
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            return _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes);
        }

        public async Task<PropertyTypeSaveViewModel> GetByIdAsync(int id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            return _mapper.Map<PropertyTypeSaveViewModel>(propertyType); // Ahora mapea a PropertyTypeSaveViewModel
        }

        public async Task<int> GetPropertyCountByTypeIdAsync(int typeId)
        {
            return await _propertyTypeRepository.GetPropertyCountByTypeIdAsync(typeId);
        }


        public async Task CreateAsync(PropertyTypeSaveViewModel propertyTypeSaveViewModel)
        {
            var propertyType = _mapper.Map<PropertyType>(propertyTypeSaveViewModel);
            await _propertyTypeRepository.AddAsync(propertyType);
        }

        public async Task EditAsync(int id, PropertyTypeSaveViewModel propertyTypeSaveViewModel)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            if (propertyType != null)
            {
                propertyType.Name = propertyTypeSaveViewModel.Name;
                propertyType.Description = propertyTypeSaveViewModel.Description;
                await _propertyTypeRepository.UpdateAsync(propertyType);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            if (propertyType != null)
            {
                // Eliminar todas las propiedades asociadas
                await _propertyTypeRepository.DeleteAsync(propertyType);
            }
        }

    }


}


