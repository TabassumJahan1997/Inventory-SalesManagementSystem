using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using DatabaseModels.ViewModels.ProductViewModels;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderDetailsService orderDetailsService;
        public OrderService(IUnitOfWork unitOfWork, IOrderDetailsService orderDetailsService)
        {
            this.unitOfWork = unitOfWork;
            this.orderDetailsService = orderDetailsService;
        }


        // ----------- GET ALL NOT DELETED ORDERS

        public async Task<IEnumerable<Order>> GetAllNotDeletedOrders()
        {
            var notDeletedOrders = await unitOfWork.OrderRepository.GetAllNotDeletedOrders();

            if(notDeletedOrders.Any())
                return notDeletedOrders;
            else
                return Enumerable.Empty<Order>();
        }


        // -------- GET LAST CREATED ORDER

        public async Task<Order> GetLastCreatedOrder()
        {
            var lastCreatedOrder = await unitOfWork.OrderRepository.GetLastCreatedOrder();

            return lastCreatedOrder;
        }

        // ----------- GET ALL ORDERS

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await unitOfWork.OrderRepository.GetAllAsync();
        }

        public async Task<bool> CreateSalesOrder(CreateSalesOrderVM data)
        {
            List<ProductOrderVM> availableOrderList = new List<ProductOrderVM>();

            foreach (var item in data.OrderedProductList)
            {
                // find out the ordered product from tblProduct
                var orderedProduct = await unitOfWork.ProductRepository
                    .GetByIdAsync(item.ProductId);

                // check if the product quantity is available
                bool isAvailable = unitOfWork.OrderRepository
                    .IsAvailable(item.OrderQuantity, orderedProduct.Quantity);

                double orderPrice = 0.00;

                // if ordered quantity is available count the total price
                if (isAvailable)
                {
                    orderPrice = unitOfWork.OrderRepository
                        .CalculateProductWiseOrderPrice(item.OrderQuantity, orderedProduct.Price);

                    availableOrderList.Add(new ProductOrderVM
                    {
                        ProductId = item.ProductId,
                        OrderQuantity = item.OrderQuantity,
                        TotalPrice = orderPrice
                    });
                }
                else
                {
                    availableOrderList = null;
                    break;
                }
            }

            if (availableOrderList == null)
                return false;

            try
            {
                await unitOfWork.BeginTransactionAsync();

                // ----------insert into tblOrder---------

                Order order = new()
                {
                    CustomerId = data.CustomerId,
                    TotalCost = availableOrderList.Sum(x => x.TotalPrice)
                };
                await unitOfWork.OrderRepository.AddAsync(order);
                await unitOfWork.CompleteAsync();


                // -----------insert into tblOrderDetails-----------

                List<OrderDetails> orderDetailsList = new();
                availableOrderList.ForEach(x =>
                {
                    orderDetailsList.Add(new OrderDetails
                    {
                        OrderId = order.OrderId,
                        ProductId = x.ProductId,
                        Quantity = x.OrderQuantity
                    });
                });
                await unitOfWork.OrderDetailsRepository
                                .AddRangeAsync(orderDetailsList);


                // ----------update tblProduct------------

                availableOrderList.ForEach(async x =>
                {
                    var productToUpdate = await unitOfWork.ProductRepository.GetByIdAsync(x.ProductId);

                    productToUpdate.Quantity -= x.OrderQuantity;

                    if(productToUpdate.Quantity <= 0)
                    {
                        productToUpdate.IsAvailable = false;
                    }

                    unitOfWork.ProductRepository
                              .Update(productToUpdate);

                });

                await unitOfWork.CompleteAsync();
                await unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                await unitOfWork.RollBackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> UpdateSalesOrder(UpdateSalesOrderVM data)
        {
            // declare variables 

            var orderedProductList = new List<Product>();
            var orderedProduct = new Product();

            var productOrderVmList = new List<ProductOrderVM>();    
            var productOrderVm = new ProductOrderVM();  

            var orderDetailsDataList = new List<OrderDetails>();
            var orderDetails = new OrderDetails();

            // ---- get order data 

            var orderDataToUpdate = await unitOfWork.OrderRepository
                .GetByIdAsync(data.OrderId);

            // ----- get order details data by order id

            var orderDetailsToDelete = await unitOfWork.OrderDetailsRepository
                .GetOrderDetailsListByOrderId(data.OrderId);

            // ------ get ordered product list data

            foreach(var ordProduct in data.OrderedProductList)
            {
                orderedProduct = unitOfWork.ProductRepository
                    .GetById(ordProduct.ProductId);

                if(orderedProduct == null)
                    return false;

                orderedProductList.Add(orderedProduct);

                // ---- get order details data by product id

                orderDetails = orderDetailsToDelete
                    .Where(x=>x.ProductId == ordProduct.ProductId)
                    .FirstOrDefault();

                int nowAvailableQuatity;

                if (orderDetails == null)
                {
                    nowAvailableQuatity = orderedProduct.Quantity  - ordProduct.OrderQuantity;
                }
                else
                {
                    nowAvailableQuatity = (orderedProduct.Quantity + orderDetails!.Quantity) - ordProduct.OrderQuantity;
                }

                // ---- check product availability

                bool isProductAvailable = unitOfWork.OrderRepository
                    .IsAvailable(ordProduct.OrderQuantity, nowAvailableQuatity);

                if (!isProductAvailable)
                {
                    productOrderVmList = null;
                    break;
                }

                // ---- update tblProduct 

                orderedProduct.Quantity = nowAvailableQuatity;

                unitOfWork.ProductRepository.Update(orderedProduct);
                unitOfWork.SaveChanges();

                // ---- populate productOrderVm and List

                productOrderVm = new ProductOrderVM
                {
                    ProductId = ordProduct.ProductId,
                    OrderQuantity = ordProduct.OrderQuantity,
                    TotalPrice = (ordProduct.OrderQuantity * orderedProduct.Price)
                };

                if (productOrderVm == null)
                    return false;

                productOrderVmList.Add(productOrderVm);

            }

            if(productOrderVmList == null) 
                return false;

            try
            {
                // ----- update tblOrder

                orderDataToUpdate.OrderId = data.OrderId;
                orderDataToUpdate.TotalCost = productOrderVmList.Sum(x => x.TotalPrice);

                if(orderDataToUpdate != null)
                {
                    unitOfWork.OrderRepository.Update(orderDataToUpdate);
                    unitOfWork.SaveChanges();
                }

                // ---- delete existing order details

                orderDetailsToDelete.ForEach( x =>
                {
                    unitOfWork.OrderDetailsRepository.Delete(x);
                    unitOfWork.SaveChanges();
                });

                // ----- insert new order details

                productOrderVmList.ForEach(x =>
                {
                    var orderDetailsData = new OrderDetails
                    {
                        OrderId = data.OrderId,
                        ProductId = x.ProductId,
                        Quantity = x.OrderQuantity
                    };


                    if (orderDetailsData != null)
                    {
                        unitOfWork.OrderDetailsRepository.Add(orderDetailsData);
                        unitOfWork.SaveChanges();
                    }
                        
                });

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        
        public async Task<bool> TempDeleteSalesOrder(int orderId)
        {
            if (orderId <= 0)
                return false;

            var orderDataToTempDelete = await unitOfWork.OrderRepository
                                        .GetByIdAsync(orderId);

            if (orderDataToTempDelete == null)
                return false;

            try
            {
                var result = await unitOfWork.OrderRepository
                            .TempDeleteOrder(orderDataToTempDelete);

                if (result)
                {
                    await unitOfWork.CompleteAsync();
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

        public async Task<bool> DeleteSalesOrder(int orderId)
        {
            if(orderId <= 0)
                return false;

            var orderToDelete = await unitOfWork.OrderRepository.GetByIdAsync(orderId);

            if (orderToDelete == null) 
                return false;

            try
            {
                var result = await unitOfWork.OrderRepository.DeleteOrder(orderToDelete);

                if (result)
                {
                    await unitOfWork.CompleteAsync();
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
