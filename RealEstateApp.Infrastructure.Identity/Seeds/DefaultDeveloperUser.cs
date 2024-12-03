using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public class DefaultDeveloperUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            ApplicationUser defaultUser = new();
            defaultUser.UserName = "developeruser";
            defaultUser.Email = "developeruser@email.com";
            defaultUser.FirstName = "Jhon";
            defaultUser.LastName = "Doe";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.UserType = Roles.Developer;
            defaultUser.Identification = "222-74322478-9";
            defaultUser.PhoneNumber = "219687892749";
            defaultUser.IsActive = true;
            defaultUser.Photo = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_640.png";



            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    // Crear usuario y asignar solo el rol de Cliente
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
        }
    }
}
