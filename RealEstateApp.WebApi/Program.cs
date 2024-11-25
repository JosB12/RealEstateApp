
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Infrastructure.Identity;

namespace RealEstateApp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
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
            //builder.Services.AddPersistenceInfrastructure(builder.Configuration);
            builder.Services.AddIdentityInfrastructureForWebApi(builder.Configuration);
            //builder.Services.AddSharedInfrastructure(builder.Configuration);
            //builder.Services.AddApplicationLayerForWebApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHealthChecks();
            //builder.Services.AddSwaggerExtension();
            //builder.Services.AddApiVersioningExtension();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
           // builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
