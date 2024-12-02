using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
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
        private readonly IUserService _userService;
        private readonly IOfferService _offerService;
        private readonly IImprovementRepository _improvementRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;


        public PropertyService(
        IUserService userService,
        IPropertyRepository propertyRepository,
        IPropertyTypeRepository propertyTypeRepository,
        IImprovementPropertyRepository improvementPropertyRepository,
        ISalesTypeRepository saleTypeRepository,
        IImprovementRepository improvementRepository,
        IImageRepository imageRepository,IOfferService offerService,
        IMapper mapper) :
            base(propertyRepository, mapper)

        {
            _propertyRepository = propertyRepository;
            _improvementPropertyRepository = improvementPropertyRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _saleTypeRepository = saleTypeRepository;
            _improvementRepository = improvementRepository;
            _userService = userService;


            _offerService = offerService;

            _imageRepository = imageRepository;
            _mapper = mapper;

        }
        #region mostrar-filtrar-Home-Crear
        public async Task<List<PropertyViewModel>> GetAvailablePropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllAsQueryable()
                                        .Where(p => p.Status == PropertyStatus.Available)
                                        .Include(p => p.Images)
                                        .Include(p => p.PropertyType)
                                        .Include(p => p.SaleType)
                                        .OrderByDescending(p => p.Created)
                                        .ToListAsync();

            var propertyViewModels = _mapper.Map<List<PropertyViewModel>>(properties);

            foreach (var property in propertyViewModels)
            {
                var entity = properties.FirstOrDefault(p => p.Id == property.Id);
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

            query = query.OrderByDescending(p => p.Created);

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
                var agent = await _userService.GetUserByIdAsync(entity.UserId);
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

            if (property == null)
            {
                return null;
            }


            var propertyViewModel = _mapper.Map<PropertySaveViewModel>(property);

            propertyViewModel.Improvements = property.Improvements?.Select(i => i.Name).ToList() ?? new List<string>();
            var agent = await _userService.GetUserByIdAsync(property.UserId);
            propertyViewModel.AgentName = agent?.FirstName + " " + agent?.LastName;
            propertyViewModel.AgentPhoneNumber = agent?.PhoneNumber;
            propertyViewModel.AgentPhotoUrl = agent?.Photo;
            propertyViewModel.AgentEmail = agent?.Email;
            propertyViewModel.ImageUrls = property.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>();
            propertyViewModel.PropertyType = property.PropertyType.Name;
            propertyViewModel.SaleType = property.SaleType.Name;

            var offers = await _offerService.GetOffersByPropertyIdAsync(id);
            propertyViewModel.Offers = offers;

            return propertyViewModel;
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
            decimal price;
            if (!decimal.TryParse(model.Price.ToString("0.##"), out price))
            {
                return new CreatePropertyResponse
                {
                    Success = false,
                    Error = "El precio ingresado no es válido."
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



        #endregion


        #region Delete property
        public async Task<bool> DeletePropertyAsync(int id)
        {
            // Obtener la propiedad que se va a eliminar
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
            {
                return false;
            }

            await _propertyRepository.DeleteAsync(property);

            return true;
        }

        public async Task<SavePropertyViewModel> GetByIdForDeleteAsync(int id)
        {
            var property = await _propertyRepository.GetAllAsQueryable()
                                                    .Include(p => p.Images)
                                                    .Include(p => p.Improvements)
                                                    .Include(p => p.PropertyType)
                                                    .Include(p => p.SaleType)
                                                    .FirstOrDefaultAsync(p => p.Id == id);

            var propertyViewModel = _mapper.Map<SavePropertyViewModel>(property);

            return propertyViewModel;
        }

        #endregion

        #region Edit Property
        public async Task<EditPropertyViewModel> GetByIdForEditAsync(int id)
        {
            // Obtén la propiedad desde el repositorio incluyendo relaciones necesarias
            var property = await _propertyRepository.GetAllAsQueryable()
                                                     .Include(p => p.Images)
                                                     .Include(p => p.Improvements)
                                                     .Include(p => p.PropertyType)
                                                     .Include(p => p.SaleType)
                                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null) return null;

            var editPropertyViewModel = new EditPropertyViewModel
            {
                PropertyTypeId = property.PropertyTypeId,
                SaleTypeId = property.SaleTypeId,
                Price = property.Price,
                Description = property.Description,
                PropertySizeMeters = property.PropertySizeMeters,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SelectedImprovements = property.Improvements?.Select(im => im.Id).ToList() ?? new List<int>(),
                CurrentImageUrls = property.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
            };

            return editPropertyViewModel;
        }


        public async Task<EditPropertyResponse> EditPropertyAsync(EditPropertyViewModel model)
        {
            var property = await _propertyRepository.GetByIdAsync(model.Id);

            if (property == null)
            {
                return new EditPropertyResponse
                {
                    Success = false,
                    ErrorMessage = "La propiedad no existe."
                };
            }

            decimal price;
            if (!decimal.TryParse(model.Price.ToString("0.##"), out price))
            {
                return new EditPropertyResponse
                {
                    Success = false,
                    ErrorMessage = "El precio ingresado no es válido."
                };
            }

            property.PropertyTypeId = model.PropertyTypeId;
            property.SaleTypeId = model.SaleTypeId;
            property.Price = model.Price;
            property.Description = model.Description;
            property.PropertySizeMeters = model.PropertySizeMeters;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;

            var currentImprovementProperties = await _improvementPropertyRepository.GetByPropertyIdAsync(property.Id);

            // Eliminar mejoras no seleccionadas
            foreach (var improvementProperty in currentImprovementProperties)
            {
                if (!model.SelectedImprovements.Contains(improvementProperty.ImprovementId))
                {
                    await _improvementPropertyRepository.DeleteAsync(improvementProperty);
                }
            }

            foreach (var improvementId in model.SelectedImprovements)
            {
                if (!currentImprovementProperties.Any(ip => ip.ImprovementId == improvementId))
                {
                    var newImprovementProperty = new ImprovementProperty
                    {
                        PropertyId = property.Id,
                        ImprovementId = improvementId
                    };
                    await _improvementPropertyRepository.AddAsync(newImprovementProperty);
                }
            }

            if (model.DeleteImages != null && model.DeleteImages.Any())
            {
                foreach (var imageUrl in model.DeleteImages)
                {
                    var imageEntity = await _imageRepository.GetByImageUrlAsync(imageUrl);
                    if (imageEntity != null)
                    {
                        var fullImagePath = Path.Combine("wwwroot", imageEntity.ImageUrl.TrimStart('/'));
                        if (File.Exists(fullImagePath))
                        {
                            File.Delete(fullImagePath);
                        }

                        await _imageRepository.DeleteAsync(imageEntity);
                    }
                }
            }

            if (model.NewImages != null && model.NewImages.Any())
            {
                if (property.Images.Count + model.NewImages.Count > 4)
                {
                    return new EditPropertyResponse
                    {
                        Success = false,
                        ErrorMessage = "No puedes agregar más de 4 imágenes."
                    };
                }

                foreach (var newImage in model.NewImages)
                {
                    var imageFileName = Guid.NewGuid() + Path.GetExtension(newImage.FileName);
                    var imagePath = Path.Combine("wwwroot/Imagenes/Propiedades", imageFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(stream);
                    }

                    var newImageEntity = new Image
                    {
                        PropertyId = property.Id,
                        ImageUrl = "/Imagenes/Propiedades/" + imageFileName
                    };

                    await _imageRepository.AddAsync(newImageEntity);
                }
            }

            await _propertyRepository.UpdateAsync(property, property.Id);

            return new EditPropertyResponse
            {
                Success = true
            };
        }



        #endregion
    }
}
