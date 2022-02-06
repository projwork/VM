using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using VendingMachine.Common;

namespace VendingMachine.Repository.Models.Helpers
{
    [UsedImplicitly]
    public class DepositDtoValidator : AbstractValidator<DepositDto>
    {
        public DepositDtoValidator()
        {
            RuleFor(v => v.Coins)
                .Must(v => v.All(coin => Constants.SupportedCoins.Contains(coin.Key)));
        }
    }
}
