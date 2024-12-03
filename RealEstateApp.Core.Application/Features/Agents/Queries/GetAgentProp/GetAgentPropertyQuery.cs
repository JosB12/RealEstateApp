using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties
{
    public class GetAgentPropertyQuery : IRequest<List<PropertyDto>>
    {
        [SwaggerParameter(Description = "Debe colocar el id del Agente del cual desea obtener las propiedades")]
        [Required]
        public string AgentId { get; set; }
    }

    public class GetAgentPropertyQueryHandler : IRequestHandler<GetAgentPropertyQuery, List<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IUserApiService _userApiService;

        public GetAgentPropertyQueryHandler(
            IPropertyRepository propertyRepository, 
            IMapper mapper,
            IUserApiService userApiService)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _userApiService = userApiService;
        }

        public async Task<List<PropertyDto>> Handle(GetAgentPropertyQuery request, CancellationToken cancellationToken)
        {
            var propertyList = await _propertyRepository
            .GetAllQueryWithInclude(new List<string> { "Improvements", "PropertyType", "SaleType" })
            .Where(p => p.UserId == request.AgentId)
            .ToListAsync();

            if (propertyList == null || propertyList.Count == 0)
                return new List<PropertyDto>();

            var properties = new List<PropertyDto>();

            foreach (var property in propertyList)
            {
                // Aquí obtenemos la información para cada propiedad de forma secuencial
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
                    Improvements = property.Improvements.Select(imp => imp.Description).ToList(),
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
