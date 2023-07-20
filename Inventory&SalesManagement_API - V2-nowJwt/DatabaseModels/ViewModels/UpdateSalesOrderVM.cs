using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels
{
    public class UpdateSalesOrderVM
    {
        [Required(ErrorMessage ="Order Id is Required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        [DisplayFormat(DataFormatString ="{0: yyyy/MMM/dd}" ,ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.Today.Date;

        [Required(ErrorMessage = "Ordered Product List is Required")]
        public ICollection<ProductOrderInputVM> OrderedProductList { get; set; } = new List<ProductOrderInputVM>();
    }
}
