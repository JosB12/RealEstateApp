using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using System.Reflection;

namespace RealEstateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayerForWebApp(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Service
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IDashboardAdminService, DashboardAdminService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IOfferService, OfferService>();

            services.AddTransient<ISalesTypeService, SalesTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();



            #endregion
        }
    }
}
