using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Controllers;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Helpers;

namespace VendingMachine.Tests.Controllers
{
    [TestClass]
    public class HelpersControllerTests
    {
        [TestMethod]
        public async Task Deposit_Always_ShouldCallUpdateDeposit()
        {
            var currentUserService = A.Fake<ICurrentUserRepository>();
            A.CallTo(() => currentUserService.UserEmail).Returns("test@gmail.com");
            A.CallTo(() => currentUserService.Deposit).Returns(5);

            var userRepository = A.Fake<IUserRepository>();


            var target = new HelpersController(currentUserService, userRepository, A.Fake<IHelperRepository>());
            var deposit = new DepositDto
            {
                Coins = new Dictionary<int, int>
                {
                    {5, 1},
                    {10, 2},
                }
            };

            await target.Deposit(deposit);

            A.CallTo(() => userRepository.UpdateDeposit(currentUserService.UserEmail, currentUserService.Deposit + 25))
                .MustHaveHappened();
        }

        [TestMethod]
        public async Task Buy_Always_ShouldCallBuy()
        {
            var helperRepository = A.Fake<IHelperRepository>();
            var target = new HelpersController(A.Fake<ICurrentUserRepository>(), A.Fake<IUserRepository>(), helperRepository);

            var param = new BuyDto();
            await target.Buy(param);

            A.CallTo(() => helperRepository.Buy(param)).MustHaveHappened();
        }
    }
}
