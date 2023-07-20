using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Abstractions;
using Repositories.Services.IServices;

namespace Inventory_SalesManagement_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsService service;
        public OrderDetailsController(IOrderDetailsService service)
        {
            this.service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetAllOrdersWithDetails()
        {
            try
            {
                var data = await service.GetAllOrderDetails();

                if (data == null)
                    return NotFound("No Order Details Available");
                else
                    return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{orderId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(int orderId)
        {
            if (orderId <= 0)
                return BadRequest(new {Message = "Invalid Order ID"});

            try
            {
                var data = await service.GetOrderDetails(orderId);

                if (data == null)
                    return NotFound("No Order Details Found With Order Id "+orderId);
                else
                    return Ok(data);

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
