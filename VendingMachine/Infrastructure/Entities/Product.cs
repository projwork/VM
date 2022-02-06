using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructure.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
        public string ProductName { get; set; }
        public string SellerId { get; set; }
    }
}
