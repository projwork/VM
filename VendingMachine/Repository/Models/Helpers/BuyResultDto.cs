using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Infrastructure.Entities;

namespace VendingMachine.Repository.Models.Helpers
{
    public class BuyResultDto
    {
        public decimal MoneySpentEur { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public ChangeDto Change { get; set; }
    }
}
