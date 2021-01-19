using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment_no_3.Models
{
    //This will be Used for Validating New Post Form 
    public class NewPost
    {
        [Required(ErrorMessage = "Please Enter your Post Title")]
        public string title { get; set; }
        [Required(ErrorMessage = "Please Enter your Post Content")]
        public string content { get; set; }
    }
}
