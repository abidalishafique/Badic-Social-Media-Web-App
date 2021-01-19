using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_no_3.Models
{
    //This will be Used for storing all kind of data related to a Post.
    public class Post
    {
        public int Id { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
        public string imagepath { get; set; }
    }
}
