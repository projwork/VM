using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;

namespace VendingMachine.Repository.Models.Products
{
    [UsedImplicitly]
    public class UpdateProductDtoValidator : AbstractValidator<UpsertProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(v => v.Id)
                .GreaterThanOrEqualTo(0);
            RuleFor(v => v.ProductName)
                .NotEmpty();
            RuleFor(v => v.AmountAvailable)
                .GreaterThanOrEqualTo(0);
            RuleFor(v => v.Cost)
                .GreaterThan(0);
        }
    }
}
