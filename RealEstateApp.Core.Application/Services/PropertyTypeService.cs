using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Services
{
    public class PropertyTypeService : GenericService<PropertyTypeViewModel, PropertyTypeViewModel, PropertyType>, IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;
        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository, IMapper mapper) : base(propertyTypeRepository, mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
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
    }
    public async Task<List<PropertyTypeViewModel>> GetAllAsync()
    {
        var propertyTypes = await _propertyTypeRepository.GetAllAsync();
        return _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes);
    }


}

}
