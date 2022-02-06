using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCaseSafe(this string valueA, string valueB)
        {
            return valueA?.Equals(valueB, StringComparison.InvariantCultureIgnoreCase)
                   ?? valueB is null;
        }
    }
}
