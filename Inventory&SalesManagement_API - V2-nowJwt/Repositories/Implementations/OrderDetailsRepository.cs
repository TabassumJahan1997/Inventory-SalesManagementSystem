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
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(Inventory_Sales_DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<OrderDetails>> GetOrderDetailsListByOrderId(int orderId)
        {
            var data = await _dbSet.Where(x=>x.OrderId == orderId).ToListAsync();
            return data;
        }

        public async Task<OrderDetails> GetOrderDetailsByOrderIdAndProductId(int orderId, int productId)
        {
            var data = await _dbSet.FirstOrDefaultAsync(x=>x.OrderId == orderId && x.ProductId == productId);
            return data;
        }

        //public async Task<List<OrderDetails>> GetAllNotDeletedOrderDetails()
        //{
        //    try
        //    {
        //        var orderDetailsList = new List<OrderDetailsVM>();

        //        // get all from tblOrderDetails

        //        var allOrderDetails = await _dbSet
        //                            .Where(x=>x.isDeleted == false)
        //                            .ToListAsync();

        //        // get a list of only distinct order Ids

        //        List<int> orderIdList = allOrderDetails
        //            .DistinctBy(x => x.OrderId)
        //            .Select(x => x.OrderId)
        //            .ToList();

        //        // push order details for each order id in orderDetailsList

        //        orderIdList.ForEach(oId =>
        //        {
        //            orderDetailsList.Add(GetOrderDetailsById(oId));
        //        });

        //        return orderDetailsList;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        

        public async Task<IEnumerable<OrderDetailsVM>> GetAllOrderDetails()
        {
            var orderDetailsList = new List<OrderDetailsVM>();

            // get all from tblOrderDetails

            var allOrderDetails = await _dbSet.ToListAsync();

            // get a list of only distinct order Ids

            List<int> orderIdList = allOrderDetails
                .DistinctBy(x => x.OrderId)
                .Select(x => x.OrderId)
                .ToList();

            // push order details for each order id in orderDetailsList

            orderIdList.ForEach(oId =>
            {
                orderDetailsList.Add(GetOrderDetailsById(oId));
            });

            return orderDetailsList;
        }

        public async Task<OrderDetailsVM> GetOrderDetails(int orderId)
        {
            // get from tblOrderDetails

            var orderDetailsData = await _dbSet
                .Where(x=>x.OrderId == orderId)
                .ToListAsync();

            // get from tblOrder

            var orderData = await _dbContext.tblOrder
                .FirstOrDefaultAsync(x=>x.OrderId == orderId);

            // get customer data from tblUser

            var customerData = await _dbContext.tblUser
                .FirstOrDefaultAsync(x=>x.UserId == orderData!.CustomerId);

            // get each product data

            List<ProductVM> orderedProductList = new List<ProductVM>();
            orderDetailsData.ForEach(x =>
            {
                // get product data from tblProduct
                var productData = _dbContext.tblProduct
                .FirstOrDefault(p=>p.ProductId == x.ProductId);

                // get product data from tblOrderDetails
                var productOrderData = _dbSet
                .FirstOrDefault(pd=>pd.ProductId == x.ProductId && 
                                    pd.OrderId == x.OrderId);   

                orderedProductList.Add(new ProductVM
                {
                    ProductId = x.ProductId,
                    ProductName = productData!.ProductName,
                    Description = productData.Description!,
                    UnitPrice = productData.Price,
                    StockQuantity = productData.Quantity,
                    OrderedQuantity = productOrderData!.Quantity
                });
            });

            // add to OrderDetailsVmData

            OrderDetailsVM OrderDetailsVmData = new()
            {
                OrderId = orderId,
                CustomerId = customerData!.UserId,
                CustomerName = customerData.UserName,
                TotalCost = orderData!.TotalCost 
            };

            orderedProductList.ForEach(x =>
            {
                OrderDetailsVmData.OrderedProducts.Add(x);
            });

            return OrderDetailsVmData;
        }

        public OrderDetailsVM GetOrderDetailsById(int orderId)
        {
            // get from tblOrderDetails

            var orderDetailsData = _dbSet
                .Where(x => x.OrderId == orderId)
                .ToList();

            // get from tblOrder

            var orderData = _dbContext.tblOrder
                .FirstOrDefault(x => x.OrderId == orderId);

            // get customer data from tblUser

            var customerData =  _dbContext.tblUser
                .FirstOrDefault(x => x.UserId == orderData!.CustomerId);

            // get each product data

            List<ProductVM> orderedProductList = new List<ProductVM>();
            orderDetailsData.ForEach(x =>
            {
                var productData = _dbContext.tblProduct
                .FirstOrDefault(p => p.ProductId == x.ProductId);

                var productOrderData = _dbSet
                .FirstOrDefault(pd => pd.ProductId == x.ProductId &&
                                    pd.OrderId == x.OrderId);

                orderedProductList.Add(new ProductVM
                {
                    ProductId = x.ProductId,
                    ProductName = productData!.ProductName,
                    Description = productData.Description!,
                    UnitPrice = productData.Price,
                    StockQuantity = productData.Quantity,
                    OrderedQuantity = productOrderData!.Quantity
                });
            });

            // add to OrderDetailsVmData

            OrderDetailsVM OrderDetailsVmData = new()
            {
                OrderId = orderId,
                CustomerId = customerData!.UserId,
                CustomerName = customerData.UserName,
                TotalCost = orderData!.TotalCost
            };

            orderedProductList.ForEach(x =>
            {
                OrderDetailsVmData.OrderedProducts.Add(x);
            });

            return OrderDetailsVmData;
        }

        public async Task<bool> TempDeleteOrderDetails(int orderId)
        {
            var orderDetailsDatae = await _dbSet.Where(x=>x.OrderId == orderId).ToListAsync();

            orderDetailsDatae.ForEach(x =>
            {
                x.isDeleted = true;
            });

            if (orderDetailsDatae.Count > 0)
            {
                _dbContext.UpdateRange(orderDetailsDatae);
                return true;
            }
            else
                return false;
        }
    }
}
