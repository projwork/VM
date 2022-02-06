using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.User
{
    public class LogoutUserDto
    {
        public string RefreshToken { get; set; }
    }
}
