using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;

namespace Inventory_SalesManagement_API.DbHostedService
{
    public class DatabaseSeederService : IHostedService
    {
        IServiceProvider serviceProvider;
        public DatabaseSeederService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using(IServiceScope scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Inventory_Sales_DbContext>();
                await SeedDbAsync(db);
            }
        }

        private async Task SeedDbAsync(Inventory_Sales_DbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.tblUser.Any() && !db.tblProduct.Any())
            {
                // tblUser
                var superAdminUser = new User
                {
                    FirstName = "SuperAdmin",
                    LastName = "SuperAdmin",
                    Email = "superAdmin@gmail.com",
                    UserName = "superAdmin",
                    Password = "D4gu0RDhqRtUph1N2hPZjg8KfeeuIdC92JuGdk502rFip5vS",
                    Role = "Super Admin"
                };
                await db.tblUser.AddAsync(superAdminUser);

                // tblProduct
                var product1 = new Product
                {
                    ProductName = "Monitor",
                    Description = "Electronic Item",
                    Price = 25000.00,
                    Quantity = 20,
                    IsAvailable = true,
                    isDeleted = false
                };
                var product2 = new Product
                {
                    ProductName = "Keyboard",
                    Description = "Electronic Item",
                    Price = 550.00,
                    Quantity = 20,
                    IsAvailable = true,
                    isDeleted = false
                };

                await db.tblProduct.AddRangeAsync(product1,product2);
                await db.SaveChangesAsync();
            }
            //throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
