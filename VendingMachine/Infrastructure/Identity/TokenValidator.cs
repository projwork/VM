using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VendingMachine.Infrastructure.Identity
{
    public class TokenValidator : ITokenValidator
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TokenValidator(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> IsValid(ClaimsPrincipal principal, string refreshToken)
        {
            var storedRefreshToken = await _applicationDbContext
                .RefreshTokens
                .SingleOrDefaultAsync(rt => rt.Value == refreshToken);

            var jti = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            return storedRefreshToken is not null
                   && storedRefreshToken.ExpirationDate >= DateTime.UtcNow
                   && storedRefreshToken.IsValid
                   && storedRefreshToken.JwtId == jti;
        }

        public async Task Invalidate(string refreshToken)
        {
            var existingToken = await _applicationDbContext
                .RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Value == refreshToken);

            if (existingToken is not null)
            {
                var tokens = await _applicationDbContext
                    .RefreshTokens
                    .Where(rt => rt.UserId == existingToken.UserId)
                    .ToListAsync();

                _applicationDbContext.RefreshTokens.RemoveRange(tokens);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
