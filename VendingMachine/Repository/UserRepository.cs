using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Common.Exceptions;
using VendingMachine.Infrastructure;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Infrastructure.Enum;
using VendingMachine.Infrastructure.Identity;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.User;

namespace VendingMachine.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ITokenValidator _tokenValidator;
        private readonly ITokenParser _tokenParser;

        public UserRepository(
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenGenerator tokenGenerator,
            ITokenValidator tokenValidator,
            ITokenParser tokenParser)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
            _tokenParser = tokenParser;
            
        }

        public async Task<UserResponse> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            var userToReturn = new UserResponse()
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Deposit = user.Deposit,
                Roles = userRoles.ToList()
            };

            return userToReturn;
        }
        public async Task<List<UserResponse>> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserResponse>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var userToReturn = new UserResponse()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Deposit = user.Deposit,
                    Roles = userRoles.ToList()
                };
                userList.Add(userToReturn);
            }

            return userList;
        }

        public async Task<AuthenticationResult> CreateUserAsync(CreateUserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                return AuthenticationResult.Failure("User with this email already exists");
            }

            var user = new ApplicationUser
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Deposit = default
            };

            var createdUser = await _userManager.CreateAsync(user, userDto.Password);
            
            if (userDto.Role == Role.buyer)
            {
                var userRole = await _userManager.AddToRoleAsync(user, "buyer");
            }
            
            if (userDto.Role == Role.seller)
            {
                var userRole= await _userManager.AddToRoleAsync(user, "seller");
            }
            

            return createdUser.Succeeded
                ? await CreateTokenAuthResponse(user, false)
                : AuthenticationResult.Failure(createdUser.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<AuthenticationResult> UpdateUserAsync(UpdateUserDto userDto)
        {
            var applicationUser = await _userManager.FindByIdAsync(userDto.UserId);

            if (applicationUser == null)
            {
                return AuthenticationResult.Failure(new string[] { "User id is not found" });
            }
            applicationUser.FirstName = userDto.FirstName;
            applicationUser.LastName = userDto.LastName;

            var updatedUser = await _userManager.UpdateAsync(applicationUser);

            var userRoles = await _userManager.GetRolesAsync(applicationUser);
            foreach (var userRole in userRoles)
            {
                var removeRole = await _userManager.RemoveFromRoleAsync(applicationUser, userRole);
            }

            if (userDto.Role == Role.buyer)
            {
                await _userManager.AddToRoleAsync(applicationUser, "buyer");
            }
            
            if (userDto.Role == Role.seller)
            {
                await _userManager.AddToRoleAsync(applicationUser, "seller");
            }

            return updatedUser.Succeeded
                ? await CreateTokenAuthResponse(applicationUser, false)
                : AuthenticationResult.Failure(updatedUser.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<AuthenticationResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
                return AuthenticationResult.Failure(new string[] { "User id is not found" });
            
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var removeRole = await _userManager.RemoveFromRoleAsync(user, userRole);
                await _applicationDbContext.SaveChangesAsync();
            }
            
            var userSessions = await _applicationDbContext.RefreshTokens.Where(rt => rt.UserId == user.Id).ToListAsync();
            _applicationDbContext.RefreshTokens.RemoveRange(userSessions);
            await _applicationDbContext.SaveChangesAsync();

            var userDelete = await _userManager.DeleteAsync(user);
            
            return userDelete.Succeeded
                ? await CreateTokenAuthResponse(user, false)
                : AuthenticationResult.Failure(userDelete.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<AuthenticationResult> LoginAsync(LoginUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var isPass = await _userManager.CheckPasswordAsync(user, userDto.Password);

            if (user != null && isPass)
            {
                var alreadyLogedIn = _applicationDbContext.RefreshTokens.Any(t => t.UserId == user.Id && t.IsValid);
                if(alreadyLogedIn)
                    throw new Exception("There is already an active session using your account"); 
                return await CreateTokenAuthResponse(user, false);
            }
            return AuthenticationResult.Failure("Wrong username or password");
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(RefreshUserDto userDto)
        {
            var principal = _tokenParser.GetPrincipalFromToken(userDto.Token);

            if (principal is not null && await _tokenValidator.IsValid(principal, userDto.RefreshToken))
            {
                var user = await _userManager
                    .FindByIdAsync(principal.Claims.Single(c => c.Type == JwtRegisteredClaimNamesEx.Id).Value);

                return await CreateTokenAuthResponse(user, false);
            }

            return AuthenticationResult.Failure("Invalid token");
        }

        public async Task LogoutAsync(LogoutUserDto userDto)
        {
            await _tokenValidator.Invalidate(userDto.RefreshToken);
        }

        public async Task LogoutAll(LogoutAllUserDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user != null)
            {
                var tokens = await _applicationDbContext
                    .RefreshTokens
                    .Where(rt => rt.UserId == user.Id)
                    .ToListAsync();

                _applicationDbContext.RefreshTokens.RemoveRange(tokens);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
        
        public async Task<decimal> UpdateDeposit(string email, decimal deposit)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.Deposit = deposit;

            await _userManager.UpdateAsync(user);
            await _applicationDbContext.SaveChangesAsync();
            return user.Deposit;
        }
        
        private async Task<AuthenticationResult> CreateTokenAuthResponse(ApplicationUser user, bool wasAlreadyLoggedIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = _tokenGenerator.AccessToken(user, tokenHandler);

            var refreshToken = await _tokenGenerator.RefreshToken(user, token);

            var result = AuthenticationResult.Success(tokenHandler.WriteToken(token), refreshToken.Value);
            result.IsAlreadyLoggedIn = wasAlreadyLoggedIn;

            return result;
        }
    }
}
