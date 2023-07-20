using DatabaseModels.DatabaseContext;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Inventory_Sales_DbContext context;
        public UnitOfWork(Inventory_Sales_DbContext context)
        {
            this.context = context;
        }



        public IProductRepository ProductRepository => new ProductRepository(context);
        public IUserRepository UserRepository => new UserRepository(context);
        public IOrderRepository OrderRepository => new OrderRepository(context);
        public IOrderDetailsRepository OrderDetailsRepository => new OrderDetailsRepository(context);



        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
        public Task SaveChanges()
        {
            context.SaveChanges();
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            this.context.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }
    }
}
