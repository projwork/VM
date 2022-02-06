using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Infrastructure.Entities;

namespace VendingMachine.Infrastructure.Identity
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtConfig _jwtConfig;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenGenerator(IOptions<JwtConfig> jwtConfig, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _jwtConfig = jwtConfig.Value;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public SecurityToken AccessToken(ApplicationUser user, JwtSecurityTokenHandler tokenHandler)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.GivenName, user.FirstName),
                new (JwtRegisteredClaimNames.FamilyName, user.LastName),
                new (JwtRegisteredClaimNamesEx.Deposit, user.Deposit.ToString(CultureInfo.InvariantCulture)),
                new (JwtRegisteredClaimNamesEx.Id, user.Id)
            };

            foreach (var role in _userManager.GetRolesAsync(user).Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtConfig.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public async Task<RefreshToken> RefreshToken(ApplicationUser user, SecurityToken token)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                IsValid = true,
                ExpirationDate = DateTime.UtcNow.Add(_jwtConfig.RefreshTokenExpiration),
                Value = Guid.NewGuid().ToString()
            };

            await _applicationDbContext.RefreshTokens.AddAsync(refreshToken);
            await _applicationDbContext.SaveChangesAsync();
            return refreshToken;
        }
    }
}
