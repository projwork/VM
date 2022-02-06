using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Filters;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Helpers;

namespace VendingMachine.Controllers
{
    public class HelpersController : AuthControllerBase
    {
        private readonly ICurrentUserRepository _currentUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHelperRepository _helperRepository;

        public HelpersController(
            ICurrentUserRepository currentUserRepository,
            IUserRepository userRepository,
            IHelperRepository helperRepository)
        {
            _currentUserRepository = currentUserRepository;
            _userRepository = userRepository;
            _helperRepository = helperRepository;
        }

        [HttpPost]
        [Route("deposit")]
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult<decimal>> Deposit([FromBody] DepositDto deposit)
        {
            return await _userRepository.UpdateDeposit(_currentUserRepository.UserEmail, _currentUserRepository.Deposit + deposit.Coins.Sum(c => c.Key * c.Value));
        }

        [HttpPost]
        [Route("buy")]
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult<BuyResultDto>> Buy([FromBody] BuyDto order)
        {
            return await _helperRepository.Buy(order);
        }

        [HttpPost]
        [Route("reset")]
        [ClaimRequirement("role", "buyer")]
        public async Task<ActionResult> Reset()
        {
            await _userRepository.UpdateDeposit(_currentUserRepository.UserEmail, 0);

            return Ok("Your deposit have been reset");
        }
    }
}
