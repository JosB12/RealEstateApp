﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Core.Domain.Enums;


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



        }
    }
}
