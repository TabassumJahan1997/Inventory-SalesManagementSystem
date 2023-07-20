using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IOrderDetailsRepository:IGenericRepository<OrderDetails>
    {
        //Task<IEnumerable<OrderDetails>> GetAllNotDeletedOrderDetails();
        Task<OrderDetails> GetOrderDetailsByOrderIdAndProductId(int orderId, int productId);
        Task<OrderDetailsVM> GetOrderDetails(int orderId);
        Task<List<OrderDetails>> GetOrderDetailsListByOrderId(int orderId);
        Task<IEnumerable<OrderDetailsVM>> GetAllOrderDetails();
        OrderDetailsVM GetOrderDetailsById(int orderId);
        Task<bool> TempDeleteOrderDetails(int orderId); 

    }

    
}
