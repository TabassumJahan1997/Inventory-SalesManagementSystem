using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.ViewModels
{
    public class AssignRoleVM
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
