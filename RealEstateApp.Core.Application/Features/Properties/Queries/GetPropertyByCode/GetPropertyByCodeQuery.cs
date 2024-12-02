using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyByCode
{
    public class GetPropertyByCodeQuery : IRequest<PropertyDto>
    {
        [SwaggerParameter(Description = "Debe colocar el código de la propiedad que quiere obtener")]
        [Required]
        public string PropertyCode { get; set; }
    }

    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IWebApiAccountService _accountService;

        public GetPropertyByCodeQueryHandler(
            IPropertyRepository propertyRepository,
            IMapper mapper,
            IWebApiAccountService accountService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<PropertyDto> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository
               .GetAllQueryWithInclude(new List<string> { "Improvements", "PropertyType", "SaleType" })
               .FirstOrDefaultAsync(p => p.PropertyCode == request.PropertyCode, cancellationToken);

            if (property == null) return null;

            PropertyDto propertyDto = new()
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

            return propertyDto;
        }
    }
}
