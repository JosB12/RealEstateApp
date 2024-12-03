using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Offer;
using RealEstateApp.Core.Application.Dtos.Account.Create;
using RealEstateApp.Core.Application.Dtos.Account.EditUsers;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Dtos.PropertyType;
using RealEstateApp.Core.Application.Features.SalesTypes.Commands.UpdateSaleType;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Dtos.SaleType;
using RealEstateApp.Core.Application.Dtos.Improvement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.CreatePropertyType;
using RealEstateApp.Core.Application.Features.PropertiesTypes.Commands.UpdatePropertieType;
using RealEstateApp.Core.Application.Features.SalesTypes.Commands.CreateSaleType;
using RealEstateApp.Core.Application.Dtos.Char;
using RealEstateApp.Core.Application.ViewModels.ChatAgent;


namespace RealEstateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            #region Properties

            CreateMap<Property, PropertyViewModel>()
               .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
               .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault().ImageUrl))
               .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Name).ToList()));

            CreateMap<PropertySaveViewModel, Property>().ReverseMap();

            #endregion

            #region PropertiesType
            CreateMap<PropertyType, PropertyTypeViewModel>().ReverseMap();
            CreateMap<PropertyTypeSaveViewModel, PropertyType>().ReverseMap();
            #endregion

            #region SalesType
            CreateMap<SaleType, SaleTypeViewModel>()
            .ForMember(dest => dest.SaleTypeCount, opt => opt.Ignore());
            CreateMap<SaveSalesTypeViewModel, SaleType>().ReverseMap();


            #endregion

            #region Improvement
            CreateMap<Improvement, ImprovementViewModel>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<SaveImprovementViewModel, Improvement>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())

                .ForMember(dest => dest.LastModified, opt => opt.Ignore())

                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Properties, opt => opt.Ignore())
                .ReverseMap();



            #endregion

            #region agent

            CreateMap<AgentDto, AgentListViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.NumberOfProperties, opt => opt.MapFrom(src => src.NumberOfProperties))
             .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Property, SavePropertyViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Si no necesitas mapear las imágenes de vuelta al ViewModel
            .ForMember(dest => dest.SelectedImprovements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Id)))
            .ForMember(dest => dest.PropertyTypeId, opt => opt.MapFrom(src => src.PropertyTypeId))
            .ForMember(dest => dest.SaleTypeId, opt => opt.MapFrom(src => src.SaleTypeId))
            .ReverseMap();

            CreateMap<Property, PropertySaveViewModel>()
                .ForMember(dest => dest.PropertyCode, opt => opt.MapFrom(src => src.PropertyCode))
                .ForMember(dest => dest.PropertyTypeId, opt => opt.MapFrom(src => src.PropertyTypeId))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
                .ForMember(dest => dest.SaleTypeId, opt => opt.MapFrom(src => src.SaleTypeId))
                .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Bedrooms))
                .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.Bathrooms))
                .ForMember(dest => dest.PropertySizeMeters, opt => opt.MapFrom(src => src.PropertySizeMeters))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Name).ToList()))
                .ForMember(dest => dest.AgentName, opt => opt.Ignore())  
                .ForMember(dest => dest.AgentPhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.AgentPhotoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.AgentEmail, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl).ToList()))
                .ReverseMap();

            #endregion

            #region Admin
            CreateMap<AdminDto, AdminListViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
             .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<SaveAdminViewModel, RegisterAdminRequest>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword))
            .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRol, opt => opt.MapFrom(src => src.UserRol));

            CreateMap<EditAdminDto, EditAdminViewModel>()
            .ReverseMap();
            #endregion

            #region Developer

            CreateMap<DeveloperDto, DeveloperListViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
             .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<SaveDeveloperViewModel, RegisterDeveloperRequest>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword))
            .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRol, opt => opt.MapFrom(src => src.UserRol));

            CreateMap<EditDeveloperDto, EditDeveloperViewModel>()
            .ReverseMap();
            #endregion

            CreateMap<PropertyTypeViewModel, SalesTypeDto>();

            CreateMap<Improvement, ImprovementViewModel>().ReverseMap();

            CreateMap<Property, SavePropertyViewModel>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Improvements,
                           opt => opt.MapFrom(src => src.Improvements.Select(im => new ImprovementViewModel
                           {
                               Id = im.Id,
                               Name = im.Name
                           }).ToList()))
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
                .ReverseMap();


            CreateMap<SavePropertyViewModel, Property>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PropertyStatus.Available))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyCode, opt => opt.Ignore());

            CreateMap<IFormFile, Image>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest) => "/Imagenes/Propiedades/" + src.FileName));


            #region Favorites
            CreateMap<Favorite, PropertyViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Property.Id))
                .ForMember(dest => dest.PropertyCode, opt => opt.MapFrom(src => src.Property.PropertyCode))
                .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.Property.PropertyType.Name))
                .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.Property.SaleType.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Property.Price))
                .ForMember(dest => dest.Bedrooms, opt => opt.MapFrom(src => src.Property.Bedrooms))
                .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.Property.Bathrooms))
                .ForMember(dest => dest.PropertySizeMeters, opt => opt.MapFrom(src => src.Property.PropertySizeMeters))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Property.Images.FirstOrDefault().ImageUrl))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Property.Improvements.Select(i => i.Name).ToList()))
                .ForMember(dest => dest.AgentName, opt => opt.Ignore())
                .ForMember(dest => dest.AgentPhoneNumber, opt => opt.Ignore()) 
                .ForMember(dest => dest.AgentPhotoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.AgentEmail, opt => opt.Ignore())
                .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => true));
            #endregion

            #region Offers
            CreateMap<Offer, OfferViewModel>().ReverseMap();
            CreateMap<OfferSaveViewModel, Offer>().ReverseMap();
            #endregion


            #region Chat
            CreateMap<Chat, ChatMessageViewModel>().ReverseMap();

            CreateMap<SendMessageDto, ChatAgentAndClientsViewModel>()
                .ForMember(dest => dest.SenderId, opt => opt.Ignore()) // Ignorar SenderId ya que se asignará manualmente
                .ReverseMap();


            #endregion

            #region CQRS

            CreateMap<CreateImprovementCommand, Improvement>()
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ReverseMap();


            CreateMap<ImprovementUpdateResponse, Improvement>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();

            
            CreateMap<UpdateImprovementCommand, Improvement>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();

            
            CreateMap<CreatePropertyTypeCommand, PropertyType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();



            CreateMap<PropertyTypeUpdateResponse, PropertyType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();


            CreateMap<UpdatePropertyTypeCommand, PropertyType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();


            
            CreateMap<CreateSaleTypeCommand, SaleType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<SaleTypeUpdateResponse, SaleType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<UpdateSaleTypeCommand, SaleType>()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
            .ReverseMap();



            #endregion


            #region DTO para api

            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
                .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(imp => imp.Description).ToList()))
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                 .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                 .ForMember(x => x.LastModified, opt => opt.Ignore())
                 .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<PropertyType, SalesTypeDto>()
             .ReverseMap()
             .ForMember(x => x.Created, opt => opt.Ignore())
             .ForMember(x => x.CreatedBy, opt => opt.Ignore())
             .ForMember(x => x.LastModified, opt => opt.Ignore())
             .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<SaleType, SaleTypeDto>()
             .ReverseMap()
             .ForMember(x => x.Created, opt => opt.Ignore())
             .ForMember(x => x.CreatedBy, opt => opt.Ignore())
             .ForMember(x => x.LastModified, opt => opt.Ignore())
             .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Improvement, ImprovementDto>()
             .ReverseMap()
             .ForMember(x => x.Created, opt => opt.Ignore())
             .ForMember(x => x.CreatedBy, opt => opt.Ignore())
             .ForMember(x => x.LastModified, opt => opt.Ignore())
             .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            
            #endregion

        }
    }
}
