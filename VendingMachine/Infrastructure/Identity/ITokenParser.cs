using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure.Identity
{
    public interface ITokenParser
    {
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
