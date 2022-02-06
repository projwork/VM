using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Repository.Models.Helpers;

namespace VendingMachine.Repository.Interfaces
{
    public interface IHelperRepository
    {
        Task<BuyResultDto> Buy(BuyDto buyInformation);
    }
}
