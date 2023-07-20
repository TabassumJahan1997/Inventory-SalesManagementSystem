using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using Repositories.Services.IServices;

namespace Inventory_SalesManagement_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class OrderController : ControllerBase
    {
        
        private readonly IOrderService service;
        public OrderController( IOrderService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await service.GetAllOrders();

            if(orders != null)
                return Ok(orders);
            else
                return BadRequest(new {Message = "No order data found"});
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllNotDeletedOrders()
        {
            var orders = await service.GetAllNotDeletedOrders();

            if (orders != null)
                return Ok(orders);
            else
                return BadRequest(new { Message = "No order data found" });
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateSalesOrder(CreateSalesOrderVM vm)
        {
            try
            {
                var result = await service.CreateSalesOrder(vm);
                if (result)
                {
                    var createdOrder = await service.GetLastCreatedOrder();

                    return Ok(new
                    {
                        Message = $"Order has been created. Your order id : {createdOrder.OrderId} "
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Sorry ! Failed to create order"
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{orderId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateSalesOrder(int orderId, UpdateSalesOrderVM vm)
        {
            if (orderId == 0 || orderId != vm.OrderId)
                return BadRequest(new {Message = "Invalid Order ID"});

            try
            {
                var result = await service.UpdateSalesOrder(vm);

                if (result)
                    return Ok(new {Message = "Updated Successfully"});
                else
                    return BadRequest(new {Message = "Sorry ! Failed to update order details"});
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPut("{orderId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TempDeleteSalesOrder(int orderId)
        {
            if(orderId == 0)
                return BadRequest(new { Message = "Invalid Order Id" });

            try
            {
                var result = await service.TempDeleteSalesOrder(orderId);

                if(result)
                    return Ok(new {Message = "Order Cancelled"});
                else
                    return BadRequest(new {Message = "Failed to Cancel Order"});
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{orderId}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteSalesOrder(int orderId)
        {
            if (orderId <= 0)
                return BadRequest(new { Message = "Invalid Order ID" });

            try
            {
                var result = await service.DeleteSalesOrder(orderId);

                if (result)
                    return Ok(new { Message = "Order Deleted Permanently" });
                else
                    return BadRequest(new { Message = "Sorry ! Failed to delete order" });

            }
            catch (Exception)
            {

                throw;
            }
        }
        
     }
}
