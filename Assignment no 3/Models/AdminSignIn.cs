using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment_no_3.Models
{
    //This will be Used for Admin SignIn
    public class AdminSignIn
    { 
        [Required(ErrorMessage = "Please Enter your Username")]
        public string username { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        public string password { get; set; }
    }
}
