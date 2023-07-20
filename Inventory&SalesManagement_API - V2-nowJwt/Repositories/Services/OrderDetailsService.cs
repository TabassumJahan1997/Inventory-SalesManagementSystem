using DatabaseModels.Models;
using DatabaseModels.ViewModels;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services
{
    public class OrderDetailsService:IOrderDetailsService
    {
        private readonly IUnitOfWork unitOfWork;
        public OrderDetailsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //public async Task<IEnumerable<OrderDetails>> GetAllNotDeletedOrderDetails()
        //{
        //    try
        //    {
        //        var data = await unitOfWork.OrderDetailsRepository.GetAllNotDeletedOrderDetails();
        //        return data;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<IEnumerable<OrderDetailsVM>> GetAllOrderDetails()
        {
            try
            {
                var data = await unitOfWork.OrderDetailsRepository
                                           .GetAllOrderDetails();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<OrderDetailsVM> GetOrderDetails(int orderId)
        {
            var orderData = await unitOfWork.OrderRepository.GetByIdAsync(orderId);

            try
            {
                var data = await unitOfWork.OrderDetailsRepository
                                           .GetOrderDetails(orderId);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> TempDeleteOrderDetailsList(int orderId)
        {
            try
            {
                var result = await unitOfWork.OrderDetailsRepository
                            .TempDeleteOrderDetails(orderId);

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
