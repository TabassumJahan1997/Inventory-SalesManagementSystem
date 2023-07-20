using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetAllNotDeletedOrders();
        Task<Order> GetLastCreatedOrder();
        Task<bool> CreateSalesOrder(CreateSalesOrderVM data);
        Task<bool> UpdateSalesOrder(UpdateSalesOrderVM data);
        Task<bool> TempDeleteSalesOrder(int orderId);
        Task<bool> DeleteSalesOrder(int orderId);
    }
}
