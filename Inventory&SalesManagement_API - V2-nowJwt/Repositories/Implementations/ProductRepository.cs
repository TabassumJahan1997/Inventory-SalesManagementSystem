using DatabaseModels.DatabaseContext;
using DatabaseModels.Models;
using DatabaseModels.ViewModels.ProductViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class ProductRepository :GenericRepository<Product>, IProductRepository
    {
        
        public ProductRepository(Inventory_Sales_DbContext context) : base(context)
        {
            
        }

        
        public async Task<bool> InsertProduct(ProductInputVM product)
        {
            if(product != null)
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC sp_Insert_Update_Product @productName = '{product.ProductName}', @description = '{product.Description}', @price = {product.Price}, @quantity = {product.Quantity}, @isAvailable = {product.IsAvailable}");

                return true;
            }
            else
                return false;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            if (product != null)
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC sp_Insert_Update_Product @productId = {product.ProductId}, @productName = '{product.ProductName}', @description = '{product.Description}', @price = {product.Price}, @quantity = {product.Quantity}, @isAvailable = {product.IsAvailable}");

                return true;
            }
            else
                return false;
        }

        public async Task<bool> TempDeleteProduct(int id)
        {
            if (id != 0)
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC sp_Insert_Update_Product @productId = {id}, @isDeleted = {1}");

                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            if(id <= 0)
                return false;
            else
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC sp_Delete_Product @productId = {id}");

                return true;
            }

        }

        public async  Task<bool> IsOrderedProduct(int id)
        {
            var orderedProduct = await _dbContext.tblOrderDetails.AnyAsync(x=>x.ProductId == id);

            if (orderedProduct)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<Product>> GetAllNotDeletedProducts()
        {
            var data = await _dbContext.tblProduct
                        .Where(x => x.isDeleted == false)
                        .ToListAsync();
            
            return data;
        }

        public async Task<bool> IsDuplicateProduct(ProductInputVM product)
        {
            var existingProductData = await _dbSet
                .Where(x=>x.ProductName == product.ProductName &&
                x.Description == product.Description && 
                x.Price == product.Price && 
                x.Quantity == product.Quantity && 
                x.IsAvailable == product.IsAvailable)
                .ToListAsync();

            if(existingProductData.Count > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> AreDuplicateProducts(IEnumerable<ProductInputVM> products)
        {
            var existingProductList = new List<Product>();

            foreach (var product in products)
            {
                var existingProductData = await _dbSet
                        .Where(x => x.ProductName == product.ProductName &&
                        x.Description == product.Description &&
                        x.Price == product.Price &&
                        x.Quantity == product.Quantity &&
                        x.IsAvailable == product.IsAvailable)
                        .ToListAsync();

                if (existingProductData != null)
                    existingProductList.AddRange(existingProductData);

                if(existingProductList.Count > 0)
                    break;  
            }

            if (existingProductList.Count > 0)
                return true;
            else
                return false;
            
        }

        public async Task<bool> InsertProducts(IEnumerable<ProductInputVM> products)
        {
            var productList = new List<Product>();

            if (products != null)
            {
                foreach (var product in products)
                {
                    productList.Add(new Product
                    {
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        IsAvailable = product.IsAvailable
                    });
                }
            }
            try
            {
                if(productList.Count > 0)
                {
                    await _dbContext.tblProduct.AddRangeAsync(productList);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
