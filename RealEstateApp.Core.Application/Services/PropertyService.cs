using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;


namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyAgentGeneralViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper) : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;

        }

        //Properties availables
        public async Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetPropertiesAvailableByAgentIdAsync(agentId);

            return properties;
        }

        //All Properties 
        public async Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agentId);

            return properties;
        }
    }
}
