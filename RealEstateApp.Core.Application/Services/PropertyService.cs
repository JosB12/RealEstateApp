using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;


namespace RealEstateApp.Core.Application.Services
{
    public class PropertyService : GenericService<SavePropertyViewModel, PropertyAgentGeneralViewModel, Property>, IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IImprovementPropertyRepository _improvementPropertyRepository;
        private readonly ISalesTypeRepository _saleTypeRepository;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;
        public PropertyService(
        IPropertyRepository propertyRepository,
        IPropertyTypeRepository propertyTypeRepository,
        IImprovementPropertyRepository improvementPropertyRepository,
        ISalesTypeRepository saleTypeRepository,
        IImprovementRepository improvementRepository,
        IMapper mapper) :
            base(propertyRepository, mapper)
        {
            _propertyRepository = propertyRepository;
            _improvementPropertyRepository = improvementPropertyRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _saleTypeRepository = saleTypeRepository;
            _improvementRepository = improvementRepository;
            _mapper = mapper;
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

        //Create Property
        public async Task<CreatePropertyResponse> CreatePropertyAsync(SavePropertyViewModel model)
        {
            var hasPropertyTypes = await _propertyTypeRepository.HasAnyPropertyTypeAsync();
            var hasSaleTypes = await _saleTypeRepository.HasAnySaleTypeAsync();
            var hasImprovements = await _improvementRepository.HasAnyImprovementAsync();

            if (!hasPropertyTypes || !hasSaleTypes || !hasImprovements)
            {
                return new CreatePropertyResponse
                {
                    Success = false,
                    Error = "No hay tipos de propiedad, tipos de venta o mejoras creadas. No se puede crear la propiedad."
                };
            }

            // Validar si se seleccionaron más de 4 imágenes
            if (model.Images.Count > 4)
            {
                return new CreatePropertyResponse
                {
                    Success = false,
                    Error = "No puede seleccionar más de 4 imágenes."
                };
            }

            // Generar el código único para la propiedad
            string propertyCode = await _propertyRepository.GenerateUniquePropertyCodeAsync();

            // Mapear el SavePropertyViewModel a la entidad Property usando AutoMapper
            var property = _mapper.Map<Property>(model);
            property.PropertyCode = propertyCode;  
            property.Status = PropertyStatus.Available; 

            // Guardar la propiedad
            var createdProperty = await _propertyRepository.AddPropertyAsync(property);

            if (model.SelectedImprovements != null && model.SelectedImprovements.Any())
            {
                foreach (var improvementId in model.SelectedImprovements)
                {
                    var improvementProperty = new ImprovementProperty
                    {
                        PropertyId = createdProperty.Id,
                        ImprovementId = improvementId
                    };

                    await _improvementPropertyRepository.AddAsync(improvementProperty);
                }
            }

            // Validar y subir las imágenes
            if (model.Images != null && model.Images.Count > 0)
            {
                List<Image> images = new List<Image>();

                foreach (var file in model.Images)
                {
                    try
                    {
                        // Generar un nombre único para la imagen
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagenes", "Propiedades");

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string imagePath = Path.Combine(directoryPath, fileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        images.Add(new Image
                        {
                            PropertyId = createdProperty.Id,
                            ImageUrl = $"/Imagenes/Propiedades/{fileName}" // Guardar la URL de la imagen
                        });
                    }
                    catch (Exception ex)
                    {
                        return new CreatePropertyResponse
                        {
                            Success = false,
                            Error = $"Error al subir las imágenes: {ex.Message}"
                        };
                    }
                }

                // Guardar las imágenes en la base de datos
                await _propertyRepository.AddImagesAsync(images);
            }
            else
            {
                return new CreatePropertyResponse
                {
                    Success = false,
                    Error = "Debe subir al menos una imagen para la propiedad."
                };
            }

            return new CreatePropertyResponse
            {
                Success = true,
                PropertyId = createdProperty.Id
            };
        }

    }
}
