using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Behaviours;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using System.Reflection;

namespace RealEstateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayerForWebApp(this IServiceCollection services, IConfiguration configuration)
        {
            #region Service
            ApplicationLayerGenericService(services);
            ApplicationLayerGenericConfigurations(services);
            services.AddTransient<IUserService, UserService>();
            #endregion
        }
        public static void AddApplicationLayerForWebApi(this IServiceCollection services)
        {
            ApplicationLayerGenericService(services);
            ApplicationLayerGenericConfigurations(services);
            services.AddTransient<IUserService, UserService>();

        }
        private static void ApplicationLayerGenericService(this IServiceCollection services)
        {
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IDashboardAdminService, DashboardAdminService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IFavoriteService, FavoriteService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<ISalesTypeService, SalesTypeService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IChatService, ChatService>();
            #endregion
        }

        private static void ApplicationLayerGenericConfigurations(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion
        }
    }
}
