using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Common.Exceptions;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Products;

namespace VendingMachine.Repository
{
    public class ProductRepository :IProductRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserRepository _currentUserRepository;

        public ProductRepository(IApplicationDbContext applicationDbContext, ICurrentUserRepository currentUserRepository)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserRepository = currentUserRepository;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _applicationDbContext.Products.ToListAsync();
        }

        public async Task<Product> Create(UpsertProductDto productDto)
        {
            var product = new Product
            {
                AmountAvailable = productDto.AmountAvailable,
                Cost = productDto.Cost,
                ProductName = productDto.ProductName,
                SellerId = _currentUserRepository.UserId
            };
            if (product.Cost % 5 != 0)
            {
                throw new Exception("Product cost is not multiple of 5");
            }

            _applicationDbContext.Products.Add(product);
            await _applicationDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Update(UpsertProductDto product)
        {
            var existingProduct =
                await _applicationDbContext.Products.FirstOrDefaultAsync(c => c.ProductId == product.Id)
                ?? throw new NotFoundException(nameof(Product), product.Id);
            if (existingProduct.SellerId != _currentUserRepository?.UserId)
            {
                throw new Exception("You cannot update this product because you did not create it.");
            }
            existingProduct.AmountAvailable = product.AmountAvailable;
            existingProduct.Cost = product.Cost;
            existingProduct.ProductName = product.ProductName;

            _applicationDbContext.Products.Update(existingProduct);
            await _applicationDbContext.SaveChangesAsync();

            return existingProduct;
        }

        public async Task Delete(int id)
        {
            var existingProduct =
                await _applicationDbContext.Products.FirstOrDefaultAsync(c => c.ProductId == id)
                ?? throw new NotFoundException(nameof(Product), id);

            if (existingProduct.SellerId != _currentUserRepository?.UserId)
            {
                throw new Exception("You cannot delete this product because you did not create it.");
            }

            _applicationDbContext.Products.Remove(existingProduct);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
