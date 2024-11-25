
using RealEstateApp.Infrastructure.Identity;

namespace RealEstateApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSession();
        //builder.Services.AddPersistenceInfrastructure(builder.Configuration);
        builder.Services.AddIdentityInfrastructureForWebApp(builder.Configuration);
        //builder.Services.AddSharedInfrastructure(builder.Configuration);
        //builder.Services.AddApplicationLayerForWebApp();
        //builder.Services.AddScoped<LoginAuthorize>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseSession();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}
