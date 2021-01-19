using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Assignment_no_3.Models
{
    //This will be Used for Admin to UPdate User Data.
    public class EditUser
    {
        [Required(ErrorMessage = "Please Enter Username")]
        [StringLength(150)]
        public string username { get; set; }
        [Required(ErrorMessage = "Please Enter User's Email")]
        public string email { get; set; }
        public string password { get; set; }
        public string imagePath { get; set; }
        public string imageName { get; set; }
        public IFormFile myImage { get; set; }
    }
}
