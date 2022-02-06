using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.Helpers
{
    public class DepositDto
    {
        public Dictionary<int, int> Coins { get; set; }
    }
}
