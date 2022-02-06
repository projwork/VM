using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using VendingMachine.Infrastructure;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository.Interfaces;

namespace VendingMachine.Repository
{
    public class CurrentUserRepository : ICurrentUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserRepository(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserEmail => _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNamesEx.Id);

        public decimal Deposit => _userManager.FindByEmailAsync(UserEmail).Result.Deposit;
    }
}
