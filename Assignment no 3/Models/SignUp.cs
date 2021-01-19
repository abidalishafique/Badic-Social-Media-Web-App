using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Assignment_no_3.Models
{
    //This will be Used for User SignUP.
    public class SignUp
    {
        [Required(ErrorMessage = "Please Enter Username")]
        [StringLength(150)]
        public String username { get; set; }
        [Required(ErrorMessage = "Please Enter your Email")]
        public String email { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        public String password { get; set; }
        [Required(ErrorMessage = "Please Again Retype your Password")]
        public String retypePassword { get; set; }
        [Required(ErrorMessage = "Please Enter your Profile Photo")]
        public IFormFile myImage { get; set; }
    }
}
