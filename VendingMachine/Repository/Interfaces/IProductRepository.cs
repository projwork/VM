using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository.Models.Products;

namespace VendingMachine.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> Create(UpsertProductDto product);
        Task<Product> Update(UpsertProductDto product);
        Task Delete(int id);
    }
}
