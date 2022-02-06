using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendingMachine.Infrastructure.Entities;

namespace VendingMachine.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(e => e.ProductId)
                .ValueGeneratedOnAdd();
            builder
                .Property(e => e.Cost)
                .IsRequired();
            builder
                .Property(e => e.SellerId)
                .IsRequired();
            builder
                .Property(e => e.ProductName)
                .IsRequired();
        }
    }
}
