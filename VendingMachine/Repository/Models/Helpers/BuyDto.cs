using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.Helpers
{
    public class BuyDto
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
