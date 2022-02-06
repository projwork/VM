using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;

namespace VendingMachine.Repository.Models.Helpers
{
    [UsedImplicitly]
    public class BuyDtoValidator : AbstractValidator<BuyDto>
    {
        public BuyDtoValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0);
            RuleFor(v => v.ProductId)
                .GreaterThan(0);
        }
    }
}
