using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public TimeSpan TokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
    }
}
