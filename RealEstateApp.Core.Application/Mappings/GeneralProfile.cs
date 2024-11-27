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

            #region ApplicationUser

            #endregion
        }
    }
}
