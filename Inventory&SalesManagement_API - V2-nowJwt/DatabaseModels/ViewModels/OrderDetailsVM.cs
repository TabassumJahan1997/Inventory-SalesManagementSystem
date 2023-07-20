using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels
{
    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double TotalCost { get; set; }
        public IList<ProductVM> OrderedProducts { get; set; } = new List<ProductVM>();

    }
}
