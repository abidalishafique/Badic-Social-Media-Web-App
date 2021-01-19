using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Assignment_no_3.Models
{
    //This will be Used for User Profile
    public class Profile
    {
        [Required(ErrorMessage = "Please Enter Username")]
        [StringLength(150)]
        public string username { get; set; }
        [Required(ErrorMessage = "Please Enter your Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please Enter your Old Password")]
        public string oldPassword { get; set; }
        [Required(ErrorMessage = "Please Enter your New Password")]
        public string newPassword { get; set; }
        public string imagePath { get; set; }
        public string imageName { get; set; }
        public IFormFile myImage { get; set; }
    }
}
