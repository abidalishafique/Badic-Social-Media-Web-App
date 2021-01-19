using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assignment_no_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_no_3.Controllers
{
    public class AdminController : Controller
    {
        //This will show a form for Request of Admin SignIn
        [HttpGet]
        public IActionResult Index()
        {
            DatabaseConnector.signeduser.id = -1;
            return View();
        }
        //This will handle post request of admin signIn if admin enter correct data then
        //Admin will goto home page otherwise he will get an error message...!
        [HttpPost]
        public IActionResult Index(AdminSignIn obj)
        {
            DatabaseConnector db = new DatabaseConnector();
            if (ModelState.IsValid)
            {
                if(db.isAdmin(obj))
                {
                    DatabaseConnector.signeduser.id = 786;
                    return View("HomePage",db.getAllUsers());
                }
                else
                {
                    ModelState.AddModelError("username", "May be Username is Invalid");
                    ModelState.AddModelError("password", "May be Password is Invalid");
                    return View();
                }
                
            }
            else
            {
                return View();
            }
        }
        //This will Display Home Page of Admin.
        public IActionResult HomePage()
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                return View(db.getAllUsers());
            }
            else
                return View("Index");
        }

        //This will remove user and display Home Page of Admin.
        public IActionResult Remove(int id)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                db.DeleteUser(id);
                return View("HomePage", db.getAllUsers());
            }
            else
                return View("Index");
        }
        //This will show a form for edit the selected user. This will be a get Request
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                User user = db.getUser(id);
                DatabaseConnector.myPostId = user.id;
                EditUser editUser = new EditUser();
                editUser.username = user.username;
                editUser.email = user.email;
                editUser.imageName = user.imageName;
                if (user.imageName == "")
                {
                    editUser.imagePath = "~/ProfileImages/default.jpg";
                }
                else
                {
                    string[] arr = user.imageName.Split(".");
                    editUser.imagePath = "~/ProfileImages/" + user.username + "." + arr[1];
                }
                return View(editUser);
            }
            else
                return View("Index");
        }
        //This function will handle post request of the editUser. This will also call module
        //functions to update user in database.
        [HttpPost]
        public IActionResult EditUser(EditUser p)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                User u = db.getUser(DatabaseConnector.myPostId);
                if (u.imageName == "")
                {
                    p.imagePath = "~/ProfileImages/default.jpg";
                    p.imageName = "None";
                }
                else
                {
                    string[] arr = u.imageName.Split(".");
                    p.imagePath = "~/ProfileImages/" + u.username + "." + arr[1];
                    p.imageName = u.imageName;
                }
                if (ModelState.IsValid)
                {
                    if(p.password!=null)
                    {
                        if (p.password.Length < 8)
                        {
                            ModelState.AddModelError("newPassword", "Password must be of at least 8 Characters..!");
                            return View(p);
                        }
                    }

                    if (u.username != p.username)
                    {
                        if (!db.isUsernameValid(p.username))
                        {
                            ModelState.AddModelError("username", "Enter Usename is not Valid");
                            return View(p);
                        }
                        if (db.isUsernameExist(p.username, u.id, true))
                        {
                            ModelState.AddModelError("username", "User with this name already Exists");
                            return View(p);
                        }
                        string extension = Path.GetExtension(u.imageName);
                        string filepath = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", u.username + extension);
                        string filepath1 = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", p.username + extension);
                        FileInfo f = new FileInfo(filepath);
                        f.CopyTo(filepath1);
                        f.Delete();
                    }

                    if (u.email != p.email)
                    {
                        if (!db.checkEmail(p.email))
                        {
                            ModelState.AddModelError("email", "Entered Email is not in Correct Format");
                            return View(p);
                        }
                        if (db.isEmailExist(p.email, u.id, true))
                        {
                            ModelState.AddModelError("email", "Already Accout Exists on this Email");
                            return View(p);
                        }
                    }
                    User user = new User();
                    user.id = u.id;
                    if (p.myImage != null)
                    {
                        string filename = Path.GetFileName(p.myImage.FileName);
                        string extension = Path.GetExtension(p.myImage.FileName);
                        string extension1 = Path.GetExtension(u.imageName);
                        string filepath = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", p.username + extension);
                        string filepath1 = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", p.username + extension1);

                        FileInfo file = new FileInfo(filepath1);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        using (FileStream stream = new FileStream(filepath, FileMode.Create))
                        {
                            p.myImage.CopyTo(stream);
                        }
                        user.imageName = filename;
                    }
                    else
                        user.imageName = u.imageName;
                    if (p.password != null)
                        user.password = p.password;
                    else
                        user.password = u.password;
                    user.email = p.email;
                    user.username = p.username;
                    DatabaseConnector.signeduser = user;
                    db.UpdateProfile(user);
                    
                    return View("HomePage",db.getAllUsers());
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
        //This will show a SignUp Page where admin can add data of new User and then do a
        //post request.
        [HttpGet]
        public IActionResult CreateUser()
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                return View();
            }
            else
                return View("Index");
           
        }
        //This will handle post request of creating new user and also add user in database
        [HttpPost]
        public IActionResult CreateUser(SignUp obj)
        {
            if (DatabaseConnector.signeduser.id != -1)
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
                                            if (obj.myImage != null)
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
                                            return View("HomePage", db.getAllUsers());
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
            else
                return View("Index");
        }
        //This will handle Logout Request.
        public IActionResult LogOut()
        {
            DatabaseConnector.signeduser.id = -1;
            return View("Index");
        }
    }
}
