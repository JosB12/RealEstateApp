using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;

        }

        public async Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agentId);

            Console.WriteLine($"Properties retrieved: {properties?.Count ?? 0}");
            return properties;
        }
    }
}
