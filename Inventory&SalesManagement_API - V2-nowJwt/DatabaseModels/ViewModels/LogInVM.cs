using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels
{
    public class LogInVM
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; } = default!;
    }
}
