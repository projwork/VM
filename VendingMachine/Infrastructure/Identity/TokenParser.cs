using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Extensions;

namespace VendingMachine.Infrastructure.Identity
{
    public class TokenParser : ITokenParser
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenParser(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = default(ClaimsPrincipal);

            try
            {
                var validatedPrincipal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken
                    && jwtSecurityToken.Header.Alg.EqualsIgnoreCaseSafe(SecurityAlgorithms.HmacSha256))
                {
                    principal = validatedPrincipal;
                }
            }
            catch
            {
            }

            return principal;
        }
    }
}
