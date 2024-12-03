using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<List<PropertyDto>>  // Asegúrate de que sea List<PropertyDto>
    {
    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, List<PropertyDto>>  // Asegúrate de que la interfaz esté bien implementada
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IUserApiService _userApiService;

        public GetAllPropertiesQueryHandler(
            IPropertyRepository propertyRepository,
            IMapper mapper,
            IUserApiService userApiService) 
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _userApiService = userApiService;
        }

        public async Task<List<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)  // Debe devolver List<PropertyDto>
        {
            var propertyList = await GetAllViewModelWithFilters();
            if (propertyList == null || propertyList.Count == 0) return null;
            return propertyList;
        }

        private async Task<List<PropertyDto>> GetAllViewModelWithFilters()
        {
            var propertyList = await _propertyRepository
                .GetAllQueryWithInclude(new List<string> { "Improvements", "PropertyType", "SaleType" })
                .ToListAsync();

            var properties = new List<PropertyDto>();

            foreach (var property in propertyList)
            {
                var propertyDto = new PropertyDto
                {
                    Id = property.Id,
                    PropertyCode = property.PropertyCode,
                    PropertyTypeName = property.PropertyType.Name,
                    SaleTypeName = property.SaleType.Name,
                    Price = property.Price,
                    PropertySizeMeters = property.PropertySizeMeters,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    Description = property.Description,
                    Improvements = property.Improvements.Select(imp => imp.Name).ToList(),
                    AgentName = await _userApiService.GetUserNameByIdAsync(property.UserId),
                    AgentId = property.UserId,
                    Status = property.Status.ToString()
                };

                properties.Add(propertyDto);
            }

            return properties;
        }
    }
}
