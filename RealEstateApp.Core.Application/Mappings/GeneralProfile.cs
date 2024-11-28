using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;


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
                .ForMember(dest => dest.AgentPhoneNumber, opt => opt.Ignore()) // Remove this line if the property does not exist
                .ForMember(dest => dest.AgentPhotoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.AgentEmail, opt => opt.Ignore())
                .ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => true));
            #endregion
        }
    }
}
