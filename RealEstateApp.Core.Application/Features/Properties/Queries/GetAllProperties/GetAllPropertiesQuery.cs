//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using RealEstateApp.Core.Application.Dtos.Property;
//using RealEstateApp.Core.Application.Interfaces.Repositories;
//using RealEstateApp.Core.Application.Interfaces.Services;

//namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties
//{
//    public class GetAllPropertiesQuery : IRequest<IList<PropertyDto>>
//    {

//    }
//    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IList<PropertyDto>>
//    {
//        private readonly IPropertyRepository _propertyRepository;
//        private readonly IMapper _mapper;
//        private readonly IWebApiAccountService _accountService;

//        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, 
//            IMapper mapper,
//            IWebApiAccountService accountService)
//        {
//            _propertyRepository = propertyRepository;
//            _mapper = mapper;
//            _accountService = accountService;
//        }
//        public async Task<IList<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
//        {
//            var propertyList = await GetAllViewModelWithFilters();
//            if (propertyList == null || propertyList.Count == 0) return null;
//            return propertyList;
//        }

//        private async Task<List<PropertyDto>> GetAllViewModelWithFilters()
//        {
//            var propertyList = await _propertyRepository
//                .GetAllQueryWithInclude(new List<string> { "Improvement", "PropertyType", "SaleType" })
//                .ToListAsync(); 

//            var properties = await Task.WhenAll(propertyList.Select(async property => new PropertyDto
//            {
//                Id = property.Id,
//                PropertyCode = property.PropertyCode,
//                PropertyTypeName = property.PropertyType.Name,
//                SaleTypeName = property.SaleType.Name,
//                Price = property.Price,
//                PropertySizeMeters = property.PropertySizeMeters,
//                Bedrooms = property.Bedrooms,
//                Bathrooms = property.Bathrooms,
//                Description = property.Description,
//                Improvements = property.Improvements.Select(imp => imp.Name).ToList(),
//                AgentName = await _accountService.GetUserNameByIdAsync(property.UserId), 
//                AgentId = property.UserId,
//                Status = property.Status.ToString(),
//            }));

//            return properties.ToList();
//        }


//    }
//}
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<IList<PropertyDto>>
    {
    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IList<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IWebApiAccountService _accountService;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository,
            IMapper mapper,
            IWebApiAccountService accountService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<IList<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
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
                    AgentName = await _accountService.GetUserNameByIdAsync(property.UserId), 
                    AgentId = property.UserId,
                    Status = property.Status.ToString()
                };

                properties.Add(propertyDto);
            }

            return properties;
        }

    }
}
