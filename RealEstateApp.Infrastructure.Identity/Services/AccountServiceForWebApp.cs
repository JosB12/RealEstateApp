using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Dtos.Account.Get;
using RealEstateApp.Core.Application.Dtos.Update;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.Text;
using RealEstateApp.Core.Application.Dtos.Account.Create;
using RealEstateApp.Core.Application.Dtos.Account.EditUsers;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Application.Dtos.Account.Edit;


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
                     IPropertyRepository propertyRepository)
                   
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
            if (user.UserType == Roles.Developer)
            {
                response.HasError = true;
                response.Error = $"El rol {user.UserType} no tiene permiso para entrar en la WebApp";
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


        

       

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion

        #region register (JoinApp)
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
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
                Photo = request.PhotoUrl,
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

        #region DashboardAdmin
        public async Task<int> GetTotalAgentAssetsAsync()
        {
            var agentAssets = await _userManager.Users
                .Where(a => a.IsActive)
                .ToListAsync();

            int count = 0;
            foreach (var agent in agentAssets)
            {
                if (await _userManager.IsInRoleAsync(agent, "Agent"))

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
        //base
        public async Task<List<AgentDto>> GetAllAgentsAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.UserType == Roles.Agent) 
                .OrderBy(u => u.Id) 
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
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Admin"))
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
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Admin"))
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

        #region Admin

        public async Task<List<AdminDto>> GetAllAdminsAsync()
        {
            
            var users = await _userManager.Users
                .Where(u => u.UserType == Roles.Admin) 
                .OrderBy(u => u.Id) 
                .ToListAsync();

            var adminDtoList = new List<AdminDto>();

            foreach (var user in users)
            {
                
                adminDtoList.Add(new AdminDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Identification = user.Identification,
                    Email = user.Email,
                    IsActive = user.IsActive
                });
            }

            return adminDtoList;
        }
        public async Task<RegisterAdminResponse> CreateAdminAsync(RegisterAdminRequest request)
        {
            var response = new RegisterAdminResponse { HasError = false };

            // Verificar si el nombre de usuario ya existe
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                response.HasError = true;
                response.Error = "El nombre de usuario ya está en uso.";
                return response;
            }

            // Crear el objeto ApplicationUser
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Identification = request.Identification,
                IsActive = true,
                UserType = Roles.Admin,
            };

            
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }

            // Asignar el rol de administrador
            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

            if (!roleResult.Succeeded)
            {
                response.HasError = true;
                response.Error = "Hubo un error al asignar el rol de administrador.";
                return response;
            }
            return response;
        }

        public async Task<EditAdminDto> GetAdminForEditAsync(string adminId)
        {
            var user = await _userManager.FindByIdAsync(adminId);
            if (user == null) return null;

            return new EditAdminDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Identification = user.Identification,
                Email = user.Email,
                UserName = user.UserName,

            };
        }

        public async Task<UpdateUserResponse> UpdateAdminAsync(EditAdminViewModel vm, string loggedInUserId)
        {
            // Verificar si el usuario que se quiere editar es el mismo que el logueado
            if (vm.Id == loggedInUserId)
            {
                // Si el usuario logueado es el mismo, no se permite editar su propia cuenta
                var loggedUser = await _userManager.FindByIdAsync(loggedInUserId);
                if (loggedUser != null && await _userManager.IsInRoleAsync(loggedUser, "Admin"))
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "El administrador logueado no puede editar su propia cuenta"
                    };
                }
            }

            // Obtener el usuario por su Id
            var admin = await _userManager.FindByIdAsync(vm.Id);
            if (admin == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Administrador no encontrado"
                };
            }

            admin.FirstName = vm.FirstName;
            admin.LastName = vm.LastName;
            admin.Identification =  vm.Identification;
            admin.Email =  vm.Email;
            admin.UserName =  vm.UserName;

            if (!string.IsNullOrWhiteSpace(vm.Password) && !string.IsNullOrWhiteSpace(vm.ConfirmPassword))
            {
                if (vm.Password == vm.ConfirmPassword)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(admin);
                    var passwordResult = await _userManager.ResetPasswordAsync(admin, token, vm.Password);
                    if (!passwordResult.Succeeded)
                    {
                        return new UpdateUserResponse
                        {
                            HasError = true,
                            Error = "Error al actualizar la contraseña"
                        };
                    }
                }
                else
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "La contraseña y su confirmación no coinciden"
                    };
                }
            }

            // Intentar actualizar los cambios del usuario
            var result = await _userManager.UpdateAsync(admin);
            if (!result.Succeeded)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Error al actualizar el administrador"
                };
            }

            // Retornar respuesta exitosa
            return new UpdateUserResponse
            {
                HasError = false
            };
        }





        #endregion

        #region Developer
        public async Task<List<DeveloperDto>> GetlAllDeveloperAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.UserType == Roles.Developer)
                .OrderBy(u => u.Id)
                .ToListAsync();

            var developerDtoList = new List<DeveloperDto>();

            foreach (var user in users)
            {

                developerDtoList.Add(new DeveloperDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Identification = user.Identification,
                    Email = user.Email,
                    IsActive = user.IsActive
                });
            }

            return developerDtoList;

        }

        public async Task<RegisterDeveloperResponse> CreateDeveloperAsync(RegisterDeveloperRequest request)
        {
            var response = new RegisterDeveloperResponse { HasError = false };

            // Verificar si el nombre de usuario ya existe
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                response.HasError = true;
                response.Error = "El nombre de usuario ya está en uso.";
                return response;
            }

            // Crear el objeto ApplicationUser
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Identification = request.Identification,
                IsActive = true,
                UserType = Roles.Developer,
            };


            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }

            // Asignar el rol de administrador
            var roleResult = await _userManager.AddToRoleAsync(user, "Developer");

            if (!roleResult.Succeeded)
            {
                response.HasError = true;
                response.Error = "Hubo un error al asignar el rol de Developer.";
                return response;
            }
            return response;
        }
        public async Task<EditDeveloperDto> GetDeveloperForEditAsync(string developerId)
        {
            var user = await _userManager.FindByIdAsync(developerId);
            if (user == null) return null;

            return new EditDeveloperDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Identification = user.Identification,
                Email = user.Email,
                UserName = user.UserName,

            };
        }

        public async Task<UpdateUserResponse> UpdateDeveloperAsync(EditDeveloperViewModel vm)
        {
            
            var developer = await _userManager.FindByIdAsync(vm.Id);
            if (developer == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Administrador no encontrado"
                };
            }

            developer.FirstName =  vm.FirstName ;
            developer.LastName =  vm.LastName ;
            developer.Identification = vm.Identification ;
            developer.Email =  vm.Email ;
            developer.UserName =  vm.UserName ;

            // Si se proporciona una nueva contraseña, se valida y actualiza
            if (!string.IsNullOrWhiteSpace(vm.Password) && !string.IsNullOrWhiteSpace(vm.ConfirmPassword))
            {
                if (vm.Password == vm.ConfirmPassword)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(developer);
                    var passwordResult = await _userManager.ResetPasswordAsync(developer, token, vm.Password);
                    if (!passwordResult.Succeeded)
                    {
                        return new UpdateUserResponse
                        {
                            HasError = true,
                            Error = "Error al actualizar la contraseña"
                        };
                    }
                }
                else
                {
                    return new UpdateUserResponse
                    {
                        HasError = true,
                        Error = "La contraseña y su confirmación no coinciden"
                    };
                }
            }

            // Intentar actualizar los cambios del usuario
            var result = await _userManager.UpdateAsync(developer);
            if (!result.Succeeded)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Error al actualizar el developer"
                };
            }

            // Retornar respuesta exitosa
            return new UpdateUserResponse
            {
                HasError = false
            };


        }


        #endregion


        #region Edit profile
        public async Task<EditProfileResponse> UpdateUserAsync(string userId, EditProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new EditProfileResponse { HasError = true, Error = "Usuario no encontrado." };
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.Phone;

            if (request.Photo != null && request.Photo.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Photo.FileName)}";
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagenes", "Usuarios");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string imagePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Photo.CopyToAsync(stream);
                }

                user.Photo = $"/Imagenes/Usuarios/{fileName}";
            }

            var result = await _userManager.UpdateAsync(user);

            return new EditProfileResponse
            {
                HasError = !result.Succeeded,
                Error = result.Succeeded ? null : "Error al actualizar el perfil."
            };
        }

        public async Task<EditProfileRequest> GetUserByIdForEditiAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new EditProfileRequest
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
            };
        }

        #endregion


        #region userById
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
        #endregion




    }
}
