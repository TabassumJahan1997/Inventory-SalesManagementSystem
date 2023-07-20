using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseModels.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int UserId { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        public string? Token { get; set; }

        public string? Role { get; set; }

        [EmailAddress]
        public string? Email { get; set; } = default!;

        [Required]
        [Column(TypeName = "bit")]
        public bool isDeleted { get; set; } = false;

        public virtual ICollection<Order> Orders { get; } = new List<Order>();
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public int AccessTokenExpirationSeconds { get; set; }
    }
}
