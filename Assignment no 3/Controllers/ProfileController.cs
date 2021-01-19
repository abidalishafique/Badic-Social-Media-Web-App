using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assignment_no_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_no_3.Controllers
{
    public class ProfileController : Controller
    {
        //This will show Profile of user in form and also provide the functionality of
        //Update the Profile if entered data is Valid.
        [HttpGet]
        public IActionResult Index()
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                Profile p = new Profile();
                p.username = DatabaseConnector.signeduser.username;
                p.email = DatabaseConnector.signeduser.email;
                if (DatabaseConnector.signeduser.imageName == "")
                {
                    p.imagePath = "~/ProfileImages/default.jpg";
                    p.imageName = "None";
                }
                else
                {
                    string[] arr = DatabaseConnector.signeduser.imageName.Split(".");
                    p.imagePath = "~/ProfileImages/" + p.username + "." + arr[1];
                    p.imageName = DatabaseConnector.signeduser.imageName;
                }
                return View(p);
            }
            else
            {
                return View("../home/Index");
            } 
        }
        //This will Handle Post Request of Updating the Profile and after that show an updated
        //Profile to the User
        [HttpPost]
        public IActionResult Index(Profile p)
        {
            if (DatabaseConnector.signeduser.id != -1)
            {
                DatabaseConnector db = new DatabaseConnector();
                if (DatabaseConnector.signeduser.imageName == "")
                {
                    p.imagePath = "~/ProfileImages/default.jpg";
                    p.imageName = "None";
                }
                else
                {
                    string[] arr = DatabaseConnector.signeduser.imageName.Split(".");
                    p.imagePath = "~/ProfileImages/" + p.username + "." + arr[1];
                    p.imageName = DatabaseConnector.signeduser.imageName;
                }
                if (ModelState.IsValid)
                {
                    if (p.oldPassword != DatabaseConnector.signeduser.password)
                    {
                        ModelState.AddModelError("oldPassword", "You have Entered an Invalid Password");
                        return View(p);
                    }
                    if (p.newPassword.Length < 8)
                    {
                        ModelState.AddModelError("newPassword", "Password must be of at least 8 Characters..!");
                        return View(p);
                    }

                    if (DatabaseConnector.signeduser.username != p.username)
                    {
                        if (!db.isUsernameValid(p.username))
                        {
                            ModelState.AddModelError("username", "Enter Usename without Space");
                            return View(p);
                        }
                        if (db.isUsernameExist(p.username, DatabaseConnector.signeduser.id, true))
                        {
                            ModelState.AddModelError("username", "User with this name already Exists");
                            return View(p);
                        }
                        string extension = Path.GetExtension(DatabaseConnector.signeduser.imageName);
                        string filepath = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", DatabaseConnector.signeduser.username + extension);
                        string filepath1 = Path.Combine("C:/Users/Abid Shafique/Desktop/Assignment no 3/Assignment no 3/wwwroot/ProfileImages/", p.username + extension);
                        FileInfo f = new FileInfo(filepath);
                        f.CopyTo(filepath1);
                        f.Delete();
                    }

                    if (DatabaseConnector.signeduser.email != p.email)
                    {
                        if (!db.checkEmail(p.email))
                        {
                            ModelState.AddModelError("email", "Entered Email is not in Correct Format");
                            return View(p);
                        }
                        if (db.isEmailExist(p.email, DatabaseConnector.signeduser.id, true))
                        {
                            ModelState.AddModelError("email", "Already Accout Exists on this Email");
                            return View(p);
                        }
                    }
                    User user = new User();
                    user.id = DatabaseConnector.signeduser.id;
                    if (p.myImage != null)
                    {
                        string filename = Path.GetFileName(p.myImage.FileName);
                        string extension = Path.GetExtension(p.myImage.FileName);
                        string extension1 = Path.GetExtension(DatabaseConnector.signeduser.imageName);
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
                        user.imageName = DatabaseConnector.signeduser.imageName;
                    user.password = p.newPassword;
                    user.email = p.email;
                    user.username = p.username;
                    DatabaseConnector.signeduser = user;
                    db.UpdateProfile(user);
                    p.imageName = user.imageName;
                    if (DatabaseConnector.signeduser.imageName == "")
                    {
                        p.imagePath = "~/ProfileImages/default.jpg";
                        p.imageName = "None";
                    }
                    else
                    {
                        string[] arr = DatabaseConnector.signeduser.imageName.Split(".");
                        p.imagePath = "~/ProfileImages/" + p.username + "." + arr[1];
                        p.imageName = DatabaseConnector.signeduser.imageName;
                    }
                    return View(p);
                }
                else
                {
                    return View(p);
                }
            }
            else
            {
                return View("../home/Index");
            }
        }
    }
}
