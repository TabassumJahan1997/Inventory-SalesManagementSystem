using DatabaseModels.Models;
using DatabaseModels.ViewModels.ProductViewModels;
using Repositories.Abstractions;
using Repositories.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services
{
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await unitOfWork.ProductRepository.GetAllAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await unitOfWork.ProductRepository.GetByIdAsync(id);
        }


        //public async Task<bool> InsertProductWithSp(ProductInputVM product)
        //{
        //    var isDuplicateProduct = await unitOfWork.ProductRepository
        //                            .IsDuplicateProduct(product);

        //    if (isDuplicateProduct)
        //        return false;

        //    try
        //    {
        //        var isInserted = await unitOfWork.ProductRepository
        //                        .InsertProduct(product);

        //        if (isInserted)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public async Task<string> InsertProductWithSp(ProductInputVM product)
        {
            string msg = string.Empty;

            var isDuplicateProduct = await unitOfWork.ProductRepository
                                    .IsDuplicateProduct(product);

            if (isDuplicateProduct)
                return msg = "Duplicate Product Entry !!";

            try
            {
                var isInserted = await unitOfWork.ProductRepository
                                .InsertProduct(product);

                if (isInserted)
                    return msg;
                else
                    return msg = "Sorry ! Failed to insert ";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateProductWithSp(Product product)
        {
            try
            {
                var isUpdated = await unitOfWork.ProductRepository.UpdateProduct(product);

                if (isUpdated)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> TempDeleteProductWithSp(int id)
        {
            bool isOrdered = await unitOfWork.ProductRepository.IsOrderedProduct(id);

            if (isOrdered)
                return false;

            try
            {
                    var isDeleted = await unitOfWork.ProductRepository
                                    .TempDeleteProduct(id);

                    if (isDeleted)
                        return true;
                    else
                        return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteProductWithSp(int id)
        {
            bool isOrdered = await unitOfWork.ProductRepository.IsOrderedProduct(id);

            if (isOrdered)
                return false;

            try
            {
                var isDeleted = await unitOfWork.ProductRepository
                                .DeleteProduct(id);

                if (isDeleted)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertProducts(IEnumerable<ProductInputVM> products)
        {
            var isDuplicateProduct = await unitOfWork.ProductRepository
                                    .AreDuplicateProducts(products);

            if (isDuplicateProduct)
                return false;

            try
            {
                var result = await unitOfWork.ProductRepository
                            .InsertProducts(products);

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

        public async Task<IEnumerable<Product>> GetAllNotDeletedProducts()
        {
            try
            {
                var data = await unitOfWork.ProductRepository
                            .GetAllNotDeletedProducts();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
