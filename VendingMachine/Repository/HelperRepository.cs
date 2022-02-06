using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Common;
using VendingMachine.Common.Exceptions;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Helpers;

namespace VendingMachine.Repository
{
    public class HelperRepository : IHelperRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserRepository _currentUserRepository;
        private readonly IUserRepository _userRepository;

        public HelperRepository(IApplicationDbContext applicationDbContext, ICurrentUserRepository currentUserRepository, IUserRepository userRepository)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserRepository = currentUserRepository;
            _userRepository = userRepository;
        }
        public async Task<BuyResultDto> Buy(BuyDto buyInformation)
        {
            var existingProduct =
                _applicationDbContext.Products.FirstOrDefault(c => c.ProductId == buyInformation.ProductId)
                ?? throw new NotFoundException(nameof(Product), buyInformation.ProductId);

            var totalPrice = existingProduct.Cost * buyInformation.Amount;

            if (_currentUserRepository.Deposit < existingProduct.Cost * buyInformation.Amount)
            {
                throw new BadRequestException("Not enough money on the account");
            }

            await _userRepository.UpdateDeposit(_currentUserRepository.UserEmail, _currentUserRepository.Deposit - totalPrice);

            existingProduct.AmountAvailable -= buyInformation.Amount;
            await _applicationDbContext.SaveChangesAsync();

            return new BuyResultDto
            {
                Change = CalculateChange(_currentUserRepository.Deposit),
                Product = existingProduct,
                MoneySpentEur = totalPrice,
                Amount = buyInformation.Amount
            };

        }

        public virtual ChangeDto CalculateChange(decimal amount)
        {
            var result = new ChangeDto();

            foreach (var coin in Constants.SupportedCoins.OrderByDescending(c => c).Where(c => c <= amount))
            {
                var coinCount = (int)(amount / coin);
                if (coinCount > 0)
                {
                    result.Coins[coin] = coinCount;
                }

                amount -= coinCount * coin;
            }

            return result;
        }
    }
}
