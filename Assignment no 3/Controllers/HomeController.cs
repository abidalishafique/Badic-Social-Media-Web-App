using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assignment_no_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Assignment_no_3.Controllers
{
    public class HomeController : Controller
    {
        public User signdInUser { get; set; }
        public HomeController()
        {
            signdInUser = new User();
        }
        //This will show a form for Request of User SignIn
        [HttpGet]
        public IActionResult Index()
        {
            DatabaseConnector.signeduser.id = -1;
            return View();
        }
        //This will handle post request of user signIn if user enter correct data then
        //User will goto home page otherwise he will get an error message...!
        [HttpPost]
        public IActionResult Index(SignIn obj)
        {
            DatabaseConnector db = new DatabaseConnector();
            if(ModelState.IsValid)
            {
                if(db.checkEmail(obj.email))
                {
                    signdInUser.email = obj.email;
                    signdInUser.password = obj.password;
                    signdInUser.id = -1;
                    signdInUser = db.handleSignIn(signdInUser);
                    DatabaseConnector.signeduser = signdInUser;
                    if (signdInUser.id != -1)
                    {
                        if (DatabaseConnector.signeduser.id != -1)
                        {
                            return View("HomePage", db.getAllPosts());
                        }
                        else
                            return View("Index");
                    }
                    else
                    {
                        if (db.isEmailExist(obj.email, -1, false))
                        {
                            ModelState.AddModelError("password", "You have Entered an Invalid Password");
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("email", "You have Entered an Invalid Email");
                            ModelState.AddModelError("password", "You have Entered an Invalid Password");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("email", "Entered Email is not in Correct Format");
                    return View();
                } 
            }
            else
            {
                return View();
            }
            
        }
        //This will handle request of Creating New User and show a sign up page
        [HttpGet]
        public IActionResult Index1()
        {
            return View();
        }
        //This will handle post request of Sign Up page and Add new user in database and
        //show a success message to the User
        [HttpPost]
        public IActionResult Index1(SignUp obj)
        {
            DatabaseConnector db = new DatabaseConnector();
            if (ModelState.IsValid)
            {
                if (db.isUsernameValid(obj.username))
                {
                    if (db.checkEmail(obj.email))
                    {
                        if (obj.password.Length >= 8)
                        {
                            if (obj.password == obj.retypePassword)
                            {
                                if (db.isUsernameExist(obj.username, -1, false))
                                {
                                    ModelState.AddModelError("username", "This username is already taken..!");
                                    return View();
                                }
                                else
                                {
                                    if (db.isEmailExist(obj.email, -1, false))
                                    {
                                        ModelState.AddModelError("email", "Already Account Exists on this Email..!");
                                        return View();
                                    }
                                    else
                                    {
                                        User user = new User();
                                        user.username = obj.username;
                                        user.password = obj.password;
                                        user.email = obj.email;
                                        if(obj.myImage != null)
                                        {
                                            string filename = Path.GetFileName(obj.myImage.FileName);
                                            string extension = Path.GetExtension(obj.myImage.FileName);
                                            string filepath = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", user.username + extension);
                                            using (FileStream stream = new FileStream(filepath, FileMode.Create))
                                            {
                                                obj.myImage.CopyTo(stream);
                                            }
                                            user.imageName = filename;
                                        }
                                        db.AddUserInDBMS(user);
                                        return View("SuccessStatus");
                                    }

                                }
                            }
                            else
                            {
                                ModelState.AddModelError("retypePassword", "Retype Password doesn't match with Password..!");
                                return View();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("password", "Password must be of at least 8 Characters..!");
                            return View();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("email", "Entered Email is not in Correct Format");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("username", "Enter Usename without Space");
                    return View();
                } 
            }
            else
            {
                return View();
            }
        }
        //This will show Home Page Of User
        public IActionResult HomePage()
        {
            if(DatabaseConnector.signeduser.id!=-1)
            {
                DatabaseConnector db = new DatabaseConnector();
                return View(db.getAllPosts());
            }
            else
            {
                return View("Index");
            }        
        }
        //This will show form entering data for new Post
        [HttpGet]
        public IActionResult CreatePost()
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }
        //This will handle post request of Creating new post and add posts into the Databse
        //and will show all users posts.
        [HttpPost]
        public IActionResult CreatePost(NewPost obj)
        {
            DatabaseConnector db = new DatabaseConnector();
            if (DatabaseConnector.signeduser.id != -1)
            {
                if (ModelState.IsValid)
                {
                    signdInUser = DatabaseConnector.signeduser;
                    Post post = new Post();
                    post.content = obj.content;
                    post.title = obj.title;
                    post.userId = signdInUser.id;
                    post.username = signdInUser.username;
                    post.date = System.DateTime.Now.ToString("MMMM, dd yyyy");

                    db.AddNewPost(post);
                    List<Post> list = db.getAllUsersPost(signdInUser);
                    return View("UsersPost", list);
                }
                else
                    return View();
            }
            else
            {
                return View("Index");
            }
            
        }
        //This will show all posts of a specfic User who has recently singed In.
        public IActionResult UsersPost()
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                signdInUser = DatabaseConnector.signeduser;
                List<Post> list = db.getAllUsersPost(signdInUser);
                return View(list);
            }
            else
            {
                return View("Index");
            }
            
        }
        //This Will Delete the select Post and will Show Remaining Posts of SignedIn User.
        public IActionResult DeletePost(int id)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                signdInUser = DatabaseConnector.signeduser;
                db.DeletePost(id);
                List<Post> list = db.getAllUsersPost(signdInUser);
                return View("UsersPost", list);
            }
            else
            {
                return View("Index");
            }
        }
        //This will show a form for update the selected Post
        [HttpGet]
        public IActionResult UpdatePost(int id)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                DatabaseConnector.myPostId = id;
                NewPost p = db.GetPost(id);
                return View(p);
            }
            else
            {
                return View("Index");
            }
        }
        //This will handle reques of updating the Post and also update post in database.
        [HttpPost]
        public IActionResult UpdatePost(NewPost obj)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                if (ModelState.IsValid)
                {
                    DatabaseConnector db = new DatabaseConnector();
                    signdInUser = DatabaseConnector.signeduser;
                    db.UpdatePost(DatabaseConnector.myPostId, obj);
                    List<Post> list = db.getAllUsersPost(signdInUser);
                    return View("UsersPost", list);
                }
                else
                    return View();
            }
            else
            {
                return View("Index");
            }
        }
        //This will show details of selected Post.
        public IActionResult PostDetail(int id)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                signdInUser = DatabaseConnector.signeduser;
                Post p = db.GetCompletePost(id);
                if (p.userId == signdInUser.id)
                {
                    List<Post> list = db.getAllUsersPost(signdInUser);
                    return View("UsersPost", list);
                }
                else
                {
                    return View(p);
                }
            }
            else
            {
                return View("Index");
            }
        }
        //This will Handle Reques of Logout of user.
        public IActionResult LogOut()
        {
            DatabaseConnector.signeduser.id = -1;
            return View("Index");
        }

    }
}
