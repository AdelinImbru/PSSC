using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    public class OrderContext: DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Product { get; set; }

        public DbSet<OrderHeaderDto> OrderHeader { get; set; }

        public DbSet<OrderLineDto> OrderLine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(o => o.ProductId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeader").HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLine").HasKey(o => o.OrderLineId);
        }
    }
}
