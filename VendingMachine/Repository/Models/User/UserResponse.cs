using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.User
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Deposit { get; set; }
        public List<string> Roles { get; set; }
    }
}
