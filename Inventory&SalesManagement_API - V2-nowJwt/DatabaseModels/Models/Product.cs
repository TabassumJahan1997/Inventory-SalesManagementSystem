using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Product ID")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName ="money")]
        [DisplayFormat(DataFormatString ="{0: 0.00}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool IsAvailable { get; set; } = true;

        [Required]
        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; }= false;

        public virtual ICollection<OrderDetails> OrderDetails { get; } = new List<OrderDetails>();
    }
}
