using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string  ProductName { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int OrderedQuantity { get; set; }
    }
}
