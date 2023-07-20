using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0: 0.00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MMM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; } = DateTime.Today.Date;

        [Required]
        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; }= false;

        public virtual User User { get; set; } = null!;

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}
