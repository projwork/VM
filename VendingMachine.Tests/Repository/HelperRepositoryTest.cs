using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Common.Exceptions;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Helpers;

namespace VendingMachine.Tests.Repository
{
    [TestClass]
    public class HelperRepositoryTest
    {
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Buy_WrongProductId_ShouldThrowNotFoundException()
        {
            var products = new List<Product>
            {
                new()
                {
                    ProductId = 1,
                    AmountAvailable = 10
                }
            }.AsQueryable();


            var fakeDbSet = A.Fake<DbSet<Product>>(builder => builder.Implements(typeof(IQueryable<Product>)));
            fakeDbSet.Initialize(products);

            var currentUserService = A.Fake<ICurrentUserRepository>();
            A.CallTo(() => currentUserService.Deposit).Returns(0);

            var dbContext = A.Fake<IApplicationDbContext>();
            A.CallTo(() => dbContext.Products).Returns(fakeDbSet);

            var target = CreateTarget(dbContext);

            var param = new BuyDto();
            await target.Buy(param);
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public async Task Buy_NotEnoughMoney_ShouldThrowBadRequestException()
        {
            var products = new List<Product>
            {
                new()
                {
                    ProductId = 1,
                    AmountAvailable = 10,
                    Cost = 1
                }
            }.AsQueryable();


            var fakeDbSet = A.Fake<DbSet<Product>>(builder => builder.Implements(typeof(IQueryable<Product>)));
            fakeDbSet.Initialize(products);

            var currentUserService = A.Fake<ICurrentUserRepository>();
            A.CallTo(() => currentUserService.Deposit).Returns(0);

            var dbContext = A.Fake<IApplicationDbContext>();
            A.CallTo(() => dbContext.Products).Returns(fakeDbSet);

            var target = CreateTarget(dbContext);

            var param = new BuyDto { ProductId = 1, Amount = 1 };

            await target.Buy(param);
        }

        private IHelperRepository CreateTarget(
            IApplicationDbContext applicationDbContext = default,
            ICurrentUserRepository currentUserService = default,
            IUserRepository userRepository = default)
        {
            return A.Fake<HelperRepository>(
                x => x.WithArgumentsForConstructor(
                    new object[]
                    {
                        applicationDbContext ?? A.Fake<IApplicationDbContext>(),
                        currentUserService ?? A.Fake<ICurrentUserRepository>(),
                        userRepository ?? A.Fake<IUserRepository>()

                    })
            );
        }
    }
}
