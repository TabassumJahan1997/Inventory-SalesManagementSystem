using DatabaseModels.Models;
using DatabaseModels.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace Inventory_SalesManagement_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Super Admin")]
    public class ProductController : ControllerBase
    {
        
        private readonly IProductService service;
        public ProductController( IProductService service)
        {
            this.service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var data = await this.service.GetAllProducts();

                if (data == null)
                    return NotFound("No data available");
                else
                    return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotDeletedProducts()
        {
            try
            {
                var data = await service.GetAllNotDeletedProducts();

                if (data == null)
                    return NotFound();
                else
                    return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var result = await this.service.GetProductById(id);

                if(result == null)
                    return NotFound("Product Not Found !!");
                else 
                    return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductInputVM data)
        {
            if (data == null)
                return BadRequest(new {Message = "Proper data not inserted"});

            else
            {
                var result = await service.InsertProductWithSp(data);

                if (result==string.Empty)
                    return Ok(new
                    {
                        Message = "Data Inserted Successfully"
                    });
                else
                    return BadRequest(new
                    {
                        Message = result
                    });
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateProductList(List<ProductInputVM> products)
        {
            if (products == null)
                return BadRequest();

            try
            {
                var result = await service.InsertProducts(products);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (product == null)
                return BadRequest();

            try
            {
                var result = await service.UpdateProductWithSp(product);
                if (result)
                {
                    return Ok(new
                    {
                        Message = "Data Updated Successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Sorry ! Failed to update data"
                    });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> TempDeleteProduct(int id)
        {
            if (id <= 0)
                return NotFound();

            try
            {
                var result = await service.TempDeleteProductWithSp(id);

                if (result)
                    return Ok(new { Message = "Product Deleted" });
                else
                    return BadRequest(new {Message = "Failed to delete product"});
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id == 0)
                return NotFound();

            try
            {
                var result = await service.DeleteProductWithSp(id);

                if (result)
                {
                    return Ok(new
                    {
                        Message = "Data Deleted"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Sorry ! Failed to delete data"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        } 
    }
}
