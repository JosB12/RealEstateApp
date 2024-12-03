using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyById
{
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        [SwaggerParameter(Description = "Debe colocar el id de la propiedad que quiere obtener")]
        [Required]
        public int Id { get; set; }
    }

    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IUserApiService _userApiService;

        public GetPropertyByIdQueryHandler(
            IPropertyRepository propertyRepository, 
            IMapper mapper,
            IUserApiService userApiService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _userApiService = userApiService;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository
                .GetAllQueryWithInclude(new List<string> { "Improvements", "PropertyType", "SaleType" })
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (property == null) return null;

            PropertyDto propertyDto = new ()
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

            return propertyDto;
        }
    }
}
