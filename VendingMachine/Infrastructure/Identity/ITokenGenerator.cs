using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Infrastructure.Entities;

namespace VendingMachine.Infrastructure.Identity
{
    public interface ITokenGenerator
    {
        SecurityToken AccessToken(ApplicationUser user, JwtSecurityTokenHandler tokenHandler);
        Task<RefreshToken> RefreshToken(ApplicationUser user, SecurityToken token);
    }
}
