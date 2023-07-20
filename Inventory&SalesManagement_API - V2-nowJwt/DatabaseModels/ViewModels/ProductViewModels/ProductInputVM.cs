using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseModels.ViewModels.ProductViewModels
{
    public class ProductInputVM
    {
        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(50)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is Required")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0: 0.00}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Product Quantity is Required")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool IsAvailable { get; set; } = true;
    }
}
