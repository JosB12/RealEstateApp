using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : IWebAppAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPropertyRepository _propertyRepository;
        public AccountServiceForWebApp(
                     UserManager<ApplicationUser> userManager,
                     SignInManager<ApplicationUser> signInManager,
                     IEmailService emailService,
                     IOptions<JWTSettings> jwtSettings, IPropertyRepository propertyRepository)
                   
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _propertyRepository = propertyRepository;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.UserName}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.UserName}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Photo = user.Photo ?? "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_640.png";

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }


        #region DashboardAdmin
        public async Task<int> GetTotalAgentAssetsAsync()
        {
            var agentAssets = await _userManager.Users
                .Where(a => a.IsActive)
                .ToListAsync();

            int count =0;
            foreach (var agent in agentAssets)
            {
                if(await _userManager.IsInRoleAsync(agent, "Agent"))

                count++;
                
            }
            return count;
        }

        public async Task<int> GetTotalAgentInactiveAsync()
        {
            var agentAssets = await _userManager.Users
                .Where(a => !a.IsActive)
                .ToListAsync();

            int count = 0;
            foreach (var agent in agentAssets)
            {
                if (await _userManager.IsInRoleAsync(agent, "Agent"))

                    count++;

            }
            return count;

        }

        public async Task<int> GetTotalClientsAssetsAsync()
        {
            var clientAssets = await _userManager.Users
                .Where(a => a.IsActive)
                .ToListAsync();

            int count = 0;
            foreach (var client in clientAssets)
            {
                if (await _userManager.IsInRoleAsync(client, "Client"))

                    count++;

            }
            return count;
        }

        public async Task<int> GetTotalClientsInactiveAsync()
        {
            var clientAssets = await _userManager.Users
                .Where(a => !a.IsActive)
                .ToListAsync();

            int count = 0;
            foreach (var client in clientAssets)
            {
                if (await _userManager.IsInRoleAsync(client, "Client"))

                    count++;

            }
            return count;
        }

        public async Task<int> GetTotalDeveloperAssetsAsync()
        {
            var developerAssets = await _userManager.Users
                .Where(a => a.IsActive)
                .ToListAsync();

            int count = 0;
            foreach (var developer in developerAssets)
            {
                if (await _userManager.IsInRoleAsync(developer, "Developer"))

                    count++;

            }
            return count;
        }

        public async Task<int> GetTotalDeveloperInactiveAsync()
        {
            var developerAssets = await _userManager.Users
               .Where(a => !a.IsActive)
               .ToListAsync();

            int count = 0;
            foreach (var developer in developerAssets)
            {
                if (await _userManager.IsInRoleAsync(developer, "Developer"))

                    count++;

            }
            return count;
        }
        #endregion

        #region Agent
        public async Task<List<AgentDto>> GetAllAgentsAsync()
        {
            // Obtener todos los usuarios con el rol de Agente, ordenados por Id
            var users = await _userManager.Users
                .Where(u => u.UserType == Roles.Agent) // Filtra solo agentes
                .OrderBy(u => u.Id) // Ordena por Id (en orden de creación aproximado)
                .ToListAsync();

            var agentDtoList = new List<AgentDto>();

            foreach (var user in users)
            {
                // Obtén la cantidad de propiedades asignadas a cada agente
                var numberOfProperties = await _propertyRepository.GetCountByAgentIdAsync(user.Id);

                agentDtoList.Add(new AgentDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NumberOfProperties = numberOfProperties,
                    Email = user.Email,
                    IsActive = user.IsActive
                });
            }

            return agentDtoList;
        }

        public async Task<UpdateUserResponse> DeleteAgentAsync(string agentId)
        {
            // 1. Obtener el agente por su ID
            var agent = await _userManager.FindByIdAsync(agentId);
            if (agent == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "El agente no fue encontrado."
                };
            }

            // 2. Obtener las propiedades asociadas al agente
            var properties = await _propertyRepository.GetPropertiesByAgentIdAsync(agentId); // Suponiendo que tienes un método para obtener las propiedades del agente
            if (properties.Any())
            {
                // Eliminar todas las propiedades asociadas al agente
                _propertyRepository.DeleteProperties(properties); // Deberás tener un método DeleteProperties en tu repositorio
            }

            // 3. Eliminar al agente
            var result = await _userManager.DeleteAsync(agent);
            if (result.Succeeded)
            {
                return new UpdateUserResponse
                {
                    HasError = false
                };
            }

            // Si ocurrió un error al intentar eliminar el agente
            return new UpdateUserResponse
            {
                HasError = true,
                Error = "Ocurrió un error al intentar eliminar el agente."
            };
        }

        #endregion

        #region activate/desactivate users

        public async Task<UpdateUserResponse> ActivateUserAsync(string userId, string loggedInUserId)
        {
            // Misma lógica que DeactivateUserAsync
            if (userId == loggedInUserId)
            {
                var loggedUser = await _userManager.FindByIdAsync(loggedInUserId);
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Administrador"))
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El administrador logeado no puede activar su propia cuenta"
                    };
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado"
                };
            }

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UpdateUserResponse { HasError = false };
            }

            return new UpdateUserResponse
            {
                HasError = true,
                Error = "Error al tratar de activar el usuario"
            };
        }
        public async Task<UpdateUserResponse> DeactivateUserAsync(string userId, string loggedInUserId)
        {
            // Cambio en la lógica: verificar si el usuario intenta desactivar su propia cuenta
            if (userId == loggedInUserId)
            {
                var loggedUser = await _userManager.FindByIdAsync(loggedInUserId);
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Administrador"))
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El administrador logeado no puede deactivar su propia cuenta"
                    };
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Usuario no encontrado"
                };
            }

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new UpdateUserResponse { HasError = false };
            }

            return new UpdateUserResponse
            {
                HasError = true,
                Error = "Error al tratar de desactivar el usuario"
            };
        }
        #endregion

       

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
