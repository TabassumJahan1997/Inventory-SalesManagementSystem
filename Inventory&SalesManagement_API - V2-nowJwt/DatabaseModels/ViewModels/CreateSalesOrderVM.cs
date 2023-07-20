using DatabaseModels.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseModels.ViewModels
{
    public class CreateSalesOrderVM
    {
        [Required(ErrorMessage = "Customer Id is Required")]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Today.Date;

        [Required(ErrorMessage = "Ordered Product List is Required")]
        public ICollection<ProductOrderInputVM> OrderedProductList { get; set; } = new List<ProductOrderInputVM>();

    }
}
