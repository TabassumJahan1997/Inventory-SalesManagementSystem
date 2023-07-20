using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services.IServices
{
    public interface IOrderDetailsService
    {
        //Task<IEnumerable<OrderDetailsVM>> GetOrderDetails(int orderId);
        //Task<IEnumerable<OrderDetails>> GetAllNotDeletedOrderDetails();
        Task<OrderDetailsVM> GetOrderDetails(int orderId);
        Task<IEnumerable<OrderDetailsVM>> GetAllOrderDetails();
        Task<bool> TempDeleteOrderDetailsList(int orderId);
    }
}
