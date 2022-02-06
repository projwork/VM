using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Repository.Models.Products
{
    public class UpsertProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
    }
}
