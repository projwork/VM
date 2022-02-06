using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.Filters;
using VendingMachine.Infrastructure.Entities;
using VendingMachine.Repository.Interfaces;
using VendingMachine.Repository.Models.Products;

namespace VendingMachine.Controllers
{
    public class ProductsController : AuthControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        [HttpPost] 
        [ClaimRequirement("role", "seller")]
        public async Task<ActionResult<Product>> Create([FromBody] UpsertProductDto product)
        {
            return await _productRepository.Create(product);
        }

        [HttpPut]
        [ClaimRequirement("role", "seller")]
        public async Task<ActionResult<Product>> Update([FromBody] UpsertProductDto product)
        {
            return await _productRepository.Update(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.Delete(id);

            return NoContent();
        }
    }
}
