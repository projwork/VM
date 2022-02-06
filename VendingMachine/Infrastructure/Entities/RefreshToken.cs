using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public string Value { get; set; }

        public string JwtId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string UserId { get; set; }

        public bool IsValid { get; set; }

        public ApplicationUser User { get; set; }
    }
}
