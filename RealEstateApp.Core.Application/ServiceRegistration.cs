using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Behaviours;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatus;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyByCode;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllPropertyById;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using System.Reflection;

namespace RealEstateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayerForWebApp(this IServiceCollection services)
        {
            #region Service
            ApplicationLayerGenericService(services);
            ApplicationLayerGenericConfigurations(services);
            services.AddTransient<IUserService, UserService>();
            #endregion
        }
        public static void AddApplicationLayerForWebApi(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            ApplicationLayerGenericConfigurations(services);
            services.AddTransient<IUserApiService, UserApiService>();

            //QueryHandlers que dependen de IWebApiAccountService
            services.AddTransient<IRequestHandler<GetPropertyByCodeQuery, PropertyDto>, GetPropertyByCodeQueryHandler>();
            services.AddTransient<IRequestHandler<GetPropertyByIdQuery, PropertyDto>, GetPropertyByIdQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllPropertiesQuery, List<PropertyDto>>, GetAllPropertiesQueryHandler>();
            services.AddTransient<IRequestHandler<GetAllAgentsQuery, IList<AgentApiDto>>, GetAllAgentsQueryHandler>();
            services.AddTransient<IRequestHandler<GetAgentPropertyQuery, List<PropertyDto>>, GetAgentPropertyQueryHandler>();
            services.AddTransient<IRequestHandler<GetAgentsByIdQuery, AgentApiDto>, GetAgentsByIdQueryHandler>();
            services.AddTransient<IRequestHandler<ChangeAgentStatusCommand, Unit>, ChangeAgentStatusCommandHandler>();


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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion
        }
    }
}
