using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure.Identity
{
    public interface ITokenValidator
    {
        Task<bool> IsValid(ClaimsPrincipal principal, string refreshToken);
        Task Invalidate(string refreshToken);
    }
}
