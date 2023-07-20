using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Inventory_Sales_DbContext dbContext) : base(dbContext)
        {
        }

        public double CalculateProductWiseOrderPrice(int orderQuantity, double unitPrice)
        {
            try
            {
                double price = orderQuantity * unitPrice;
                return price;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetAllNotDeletedOrders()
        {
            var notDeletedOrders = await _dbSet.Where(x => x.isDeleted == false).Include(x=>x.User).ToListAsync();
            
            if (notDeletedOrders.Count <= 0)
                return Enumerable.Empty<Order>();
            else
                return notDeletedOrders;
        }

        public async Task<Order> GetLastCreatedOrder()
        {
            var lastOrder = await _dbSet.OrderBy(x=>x.OrderId).LastOrDefaultAsync();
            return lastOrder!;
        }

        public bool IsAvailable(int orderQuantity, int availableQuantity)
        {
            if (orderQuantity <= availableQuantity)
                return true;
            else
                return false;
        }

        public async Task<bool> TempDeleteOrder(Order data)
        {
            
            if(data == null)
                return false;

            var orderDetailsData = await _dbContext.tblOrderDetails.Where(x => x.OrderId == data.OrderId).ToListAsync();

            if (orderDetailsData.Count == 0)
                return false;

            try
            {
                orderDetailsData.ForEach(async x =>
                {
                    x.isDeleted = true;
                    _dbContext.tblOrderDetails.Update(x);
                    await _dbContext.SaveChangesAsync();
                });


                data.isDeleted = true;
                _dbSet.Update(data);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteOrder(Order data)
        {
            if(data == null)
                return false;

            try
            {
                _dbSet.Remove(data);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
