using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Common
{
    public class Constants
    {
        public static readonly IEnumerable<int> SupportedCoins = new[]
        {
            5, 10, 20, 50, 100
        };
    }
}
