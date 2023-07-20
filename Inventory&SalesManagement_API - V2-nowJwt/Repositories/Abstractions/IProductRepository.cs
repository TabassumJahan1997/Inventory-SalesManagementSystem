using DatabaseModels.Models;
using DatabaseModels.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllNotDeletedProducts();
        Task<bool> IsDuplicateProduct(ProductInputVM product);
        Task<bool> AreDuplicateProducts(IEnumerable<ProductInputVM> products);
        Task<bool> InsertProduct(ProductInputVM product);
        Task<bool> InsertProducts(IEnumerable<ProductInputVM> products);
        Task<bool> UpdateProduct(Product product);
        Task<bool> TempDeleteProduct(int id);
        Task<bool> DeleteProduct(int id);
        Task<bool> IsOrderedProduct(int id);
    }
}
