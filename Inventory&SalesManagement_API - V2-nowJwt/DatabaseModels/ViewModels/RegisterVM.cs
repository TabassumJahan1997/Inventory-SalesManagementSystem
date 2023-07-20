using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseModels.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is Required")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string? Email { get; set; } = default!;
    }
}
