using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;


namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : IWebAppAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IEmailService _emailService;

        public AccountServiceForWebApp(
                     UserManager<ApplicationUser> userManager,
                     SignInManager<ApplicationUser> signInManager,
                     IEmailService emailService,
                     IOptions<JWTSettings> jwtSettings, IPropertyRepository propertyRepository)
                   
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _propertyRepository = propertyRepository;
            _emailService = emailService;
        }

        
        #region login
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
            var properties = await _propertyRepository.GetPropertiesJustByAgentIdAsync(agentId); // Suponiendo que tienes un método para obtener las propiedades del agente
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
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
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
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
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
        #endregion

        #region register (JoinApp)
        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                Photo = request.PhotoUrl ?? "default_profile_image_url",
                UserName = request.UserName,
                Email = request.Email,
                UserType = request.UserType,
                Identification = request.Identification
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                //response.Error = "Error creating user.";
                response.Error = $"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }

            string role = request.UserType switch
            {
                Roles.Agent => "Agent",
                Roles.Client => "Client",
                _ => throw new ArgumentException("Tipo de usuario no válido")
            };
            await _userManager.AddToRoleAsync(user, role);

            if (request.UserType == Roles.Client)
            {
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"Please confirm your account visiting this URL {verificationUri}",
                    Subject = "Confirm registration"
                });
            }
            else if (request.UserType == Roles.Agent)
            {
                user.IsActive = false;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
            }
            else
            {
                response.HasError = true;
                response.Error = $"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }

            return response;
        }
        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Home/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.IsActive = true;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return $"Account confirmed, but failed to update status for {user.Email}.";
                }

                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming {user.Email}.";
            }
        }
        #endregion

        #region Agent (General)
        public async Task<List<AgentViewModel>> GetActiveAgentsAsync(string searchQuery = "")
        {
            var agentsQuery = _userManager.Users
                .Where(u => u.UserType == Roles.Agent && u.IsActive);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                agentsQuery = agentsQuery.Where(a =>
                     a.FirstName.ToLower().Contains(searchQuery.ToLower()) ||
                     a.LastName.ToLower().Contains(searchQuery.ToLower()));
            }

            var agents = await agentsQuery
                .OrderBy(agent => agent.FirstName)
                .ThenBy(agent => agent.LastName)
                .ToListAsync();

            var agentViewModels = agents.Select(agent => new AgentViewModel
            {
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                PhotoUrl = agent.Photo,
                Id = agent.Id
            }).ToList();

            return agentViewModels;
        }


        #endregion
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Photo = user.Photo,
                Email = user.Email
            };
        }

    }
}
