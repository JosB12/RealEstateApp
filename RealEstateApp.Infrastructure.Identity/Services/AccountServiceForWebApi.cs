
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Account.RegisterApi;
using RealEstateApp.Core.Application.Dtos.Agent;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApi : IWebApiAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private object _httpContextAccessor;
        private readonly JWTSettings _jwtSettings;
        private readonly IPropertyRepository _propertyRepository;

        public AccountServiceForWebApi(
                     UserManager<ApplicationUser> userManager,
                     SignInManager<ApplicationUser> signInManager,
                     IEmailService emailService,
                     IOptions<JWTSettings> jwtSettings,
                    IPropertyRepository propertyRepository)


        {

            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _propertyRepository = propertyRepository;

        }

        #region Authenticate
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

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
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

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.Photo = user.Photo;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }
        #endregion

        #region Register

        public async Task<RegisterResponse> RegisterAdminUserAsync(RegisterApiRequest request, ClaimsPrincipal currentUser)
        {
            RegisterResponse response = new() { HasError = false };

            var isAdminOrSuperAdmin = currentUser.IsInRole(Roles.Admin.ToString()) ||
                        currentUser.IsInRole(Roles.SuperAdmin.ToString());

            if (!isAdminOrSuperAdmin)
            {
                response.HasError = true;
                response.Error = "Only Admins or SuperAdmins can create new administrator accounts.";
                return response;
            }

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
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
                UserName = request.UserName,
                Email = request.Email,
                Identification = request.Identification,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                user.UserType = Roles.Admin;
                user.Photo = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_640.png";
            }
            else
            {
                response.HasError = true;
                response.Error = "An error occurred trying to register the user.";
            }

            return response;
        }
       

        public async Task<RegisterResponse> RegisterDeveloperUserAsync(RegisterApiRequest request, string origin)
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
                UserName = request.UserName,
                Email = request.Email,
                Identification = request.Identification,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Developer.ToString());
                user.UserType = Roles.Developer;
                user.Photo = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_640.png";

            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            return response;
        }
        #endregion


        #region Protected
        protected async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        protected RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        protected string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }
        #endregion

        #region get
        public async Task<string> GetUserNameByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<List<AgentApiDto>> GetAllAgentsForApiAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.UserType == Roles.Agent)
                .OrderBy(u => u.Id)
                .ToListAsync();

            var agentDtoList = new List<AgentApiDto>();

            foreach (var user in users)
            {
                var numberOfProperties = await _propertyRepository.GetCountByAgentIdAsync(user.Id);

                agentDtoList.Add(new AgentApiDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NumberOfProperties = numberOfProperties,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Phone = user.PhoneNumber
                });
            }

            return agentDtoList;
        }

        public async Task<AgentApiDto?> GetAgentByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == id && u.UserType == Roles.Agent)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null; 
            }

            var numberOfProperties = await _propertyRepository.GetCountByAgentIdAsync(user.Id);

            return new AgentApiDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NumberOfProperties = numberOfProperties,
                Email = user.Email,
                IsActive = user.IsActive,
                Phone = user.PhoneNumber
            };
        }

        public async Task<string> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.Id;
        }
        #endregion

        #region ChangeUserStatus
        public async Task ChangeUserStatusAsync(string userId, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user status");
            }
        }
        #endregion
    }
}
