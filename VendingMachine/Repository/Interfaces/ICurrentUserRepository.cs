using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Interfaces
{
    public interface ICurrentUserRepository
    {
        public string UserEmail { get; }
        public string UserId { get; }
        public decimal Deposit { get; }
    }
}
