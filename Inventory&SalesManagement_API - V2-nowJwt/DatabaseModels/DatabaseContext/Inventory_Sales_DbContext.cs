using DatabaseModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.DatabaseContext
{
    public class Inventory_Sales_DbContext:DbContext
    {
        public Inventory_Sales_DbContext(DbContextOptions<Inventory_Sales_DbContext> options):base(options)
        {
            
        }

        public DbSet<User> tblUser { get; set; } = default!;
        public DbSet<Product> tblProduct { get; set; } = default!;
        public DbSet<Order> tblOrder { get; set; } = default!;
        public DbSet<OrderDetails> tblOrderDetails { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderDetails>().HasKey(x=> new
            {
                x.OrderId,
                x.ProductId
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = "Server=.;Database=Inventory&SalesDb; Trusted_Connection=true;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
