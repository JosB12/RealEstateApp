﻿using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IPropertyService : IGenericService<SavePropertyViewModel, PropertyAgentGeneralViewModel, Property>
    {
        Task<List<PropertyViewModel>> GetAvailablePropertiesAsync();
        Task<List<PropertyViewModel>> FilterPropertiesAsync(PropertyFilterViewModel filter);
        Task<List<PropertyAgentGeneralViewModel>> GetPropertiesAvailableByAgentIdAsync(string agentId);
        Task<List<HomeAgentPropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
        Task<CreatePropertyResponse> CreatePropertyAsync(SavePropertyViewModel model);
        Task<PropertySaveViewModel> GetByIdSaveViewModel(int id);
        Task<SavePropertyViewModel> GetByIdForDeleteAsync(int id);
        Task<bool> DeletePropertyAsync(int id);
        Task<EditPropertyViewModel> GetByIdForEditAsync(int id);
        Task<EditPropertyResponse> EditPropertyAsync(EditPropertyViewModel model);
        Task MarkAsSoldAsync(int propertyId);
        Task<List<PropertyViewModel>> FilterAgentPropertiesAsync(PropertyFilterViewModel filter, string agentId);
    }
}
