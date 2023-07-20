using DatabaseModels.Models;
using DatabaseModels.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetAllNotDeletedProducts();
        Task<Product> GetProductById(int id);
        //Task<bool> InsertProductWithSp(ProductInputVM data);
        Task<string> InsertProductWithSp(ProductInputVM data);
        Task<bool> UpdateProductWithSp(Product product);
        Task<bool> TempDeleteProductWithSp(int id);
        Task<bool> DeleteProductWithSp(int id); 
        Task<bool> InsertProducts(IEnumerable<ProductInputVM> products);

    }
}
