using DatabaseModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllNotDeletedOrders();
        Task<Order> GetLastCreatedOrder();
        bool IsAvailable(int orderQuantity, int availableQuantity);
        double CalculateProductWiseOrderPrice(int orderQuantity, double unitPrice);
        Task<bool> TempDeleteOrder(Order data);
        Task<bool> DeleteOrder(Order data);

    }
}
