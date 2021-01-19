using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment_no_3.Models
{
    //This will be Used for User SignIn
    public class SignIn
    {
        [Required(ErrorMessage = "Please Enter your Email")]
        public String email { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        public String password { get; set; }
    }
}
