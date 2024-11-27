using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;


namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<PropertySaveViewModel, PropertyViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IWebAppAccountService _accountService;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, IWebAppAccountService accountService)
            : base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<List<PropertyViewModel>> GetAvailablePropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            var availableProperties = properties.Where(p => p.Status == PropertyStatus.Available)
                                                .OrderByDescending(p => p.Created)
                                                .ToList();
            var propertyViewModels = _mapper.Map<List<PropertyViewModel>>(availableProperties);

            foreach (var property in propertyViewModels)
            {
                var entity = availableProperties.FirstOrDefault(p => p.Id == property.Id);
                property.ImageUrl = entity?.Images?.FirstOrDefault()?.ImageUrl;
                property.PropertyType = entity?.PropertyType?.Name;
                property.SaleType = entity?.SaleType?.Name;
            }

            return propertyViewModels;
        }

        public async Task<List<PropertyViewModel>> FilterPropertiesAsync(PropertyFilterViewModel filter)
        {
            var query = _propertyRepository.GetAllAsQueryable();

            if (!string.IsNullOrEmpty(filter.PropertyCode))
            {
                query = query.Where(p => p.PropertyCode.Contains(filter.PropertyCode));
            }

            if (filter.PropertyTypeIds != null && filter.PropertyTypeIds.Any())
            {
                query = query.Where(p => filter.PropertyTypeIds.Contains(p.PropertyTypeId));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.Bedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms == filter.Bedrooms.Value);
            }

            if (filter.Bathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms == filter.Bathrooms.Value);
            }

            var properties = await query.Include(p => p.Images)
                                        .Include(p => p.Improvements)
                                        .Include(p => p.PropertyType)
                                        .Include(p => p.SaleType)
                                        .ToListAsync();
            var propertyViewModels = _mapper.Map<List<PropertyViewModel>>(properties);

            foreach (var property in propertyViewModels)
            {
                var entity = properties.FirstOrDefault(p => p.Id == property.Id);
                property.ImageUrl = entity?.Images?.FirstOrDefault()?.ImageUrl;
                property.Improvements = entity?.Improvements?.Select(i => i.Name).ToList() ?? new List<string>();
                var agent = await _accountService.GetUserByIdAsync(entity.UserId);
                property.AgentName = agent?.FirstName + " " + agent?.LastName;
                property.AgentPhoneNumber = agent?.PhoneNumber;
                property.AgentPhotoUrl = agent?.Photo;
                property.AgentEmail = agent?.Email;
                property.PropertyType = entity?.PropertyType?.Name;
                property.SaleType = entity?.SaleType?.Name;
            }

            return propertyViewModels;
        }

        public async Task<PropertySaveViewModel> GetByIdSaveViewModel(int id)
        {
            var property = await _propertyRepository.GetAllAsQueryable()
                                                    .Include(p => p.Images)
                                                    .Include(p => p.Improvements)
                                                    .Include(p => p.PropertyType)
                                                    .Include(p => p.SaleType)
                                                    .FirstOrDefaultAsync(p => p.Id == id);
            var propertyViewModel = _mapper.Map<PropertySaveViewModel>(property);

            propertyViewModel.Improvements = property.Improvements?.Select(i => i.Name).ToList() ?? new List<string>();
            var agent = await _accountService.GetUserByIdAsync(property.UserId);
            propertyViewModel.AgentName = agent?.FirstName + " " + agent?.LastName;
            propertyViewModel.AgentPhoneNumber = agent?.PhoneNumber;
            propertyViewModel.AgentPhotoUrl = agent?.Photo;
            propertyViewModel.AgentEmail = agent?.Email;
            propertyViewModel.ImageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>();
            propertyViewModel.PropertyType = property.PropertyType.Name;
            propertyViewModel.SaleType = property.SaleType.Name;

            return propertyViewModel;
        }
    }
}