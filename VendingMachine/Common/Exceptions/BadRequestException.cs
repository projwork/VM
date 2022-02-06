using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, Exception ex = default)
            : base(message, ex)
        {
        }
    }
}
