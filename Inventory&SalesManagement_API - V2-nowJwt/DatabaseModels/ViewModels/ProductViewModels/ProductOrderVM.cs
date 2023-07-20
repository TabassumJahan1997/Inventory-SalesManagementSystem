using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels.ProductViewModels
{
    public class ProductOrderVM
    {
        public int ProductId { get; set; }
        public int OrderQuantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
