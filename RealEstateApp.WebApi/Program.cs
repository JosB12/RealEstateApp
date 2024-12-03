using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Shared;
using RealEstateApp.WebApi.Extensions;
using RealEstateApp.WebApi.Middlewares;

namespace RealEstateApp.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressMapClientErrors = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddPersistenceInfrastructure(builder.Configuration);
            builder.Services.AddIdentityInfrastructureForWebApi(builder.Configuration);
            builder.Services.AddSharedInfrastructure(builder.Configuration);
            builder.Services.AddApplicationLayerForWebApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerExtension();
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExtension(app);
            }

            await app.Services.RunAsyncSeed();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler();
            app.UseHealthChecks("/health");
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            await app.RunAsync();

        }
    }
}

