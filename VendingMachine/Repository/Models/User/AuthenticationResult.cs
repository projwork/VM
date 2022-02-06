using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.User
{
    public class AuthenticationResult
    {
        public static AuthenticationResult Success(string token, string refreshToken)
        {
            return new()
            {
                Token = token,
                RefreshToken = refreshToken,
                IsSuccess = true,
                ErrorMessages = new string[] { }
            };
        }

        public static AuthenticationResult Failure(params string[] errors)
        {
            return new()
            {
                Token = string.Empty,
                RefreshToken = string.Empty,
                IsSuccess = false,
                ErrorMessages = errors
            };
        }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAlreadyLoggedIn { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
