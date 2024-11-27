using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.ViewModels;
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

            CreateMap<AgentDto, AgentListViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.NumberOfProperties, opt => opt.MapFrom(src => src.NumberOfProperties))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));



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
