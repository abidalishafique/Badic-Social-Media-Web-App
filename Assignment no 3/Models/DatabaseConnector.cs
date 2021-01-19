using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_no_3.Models
{
    public class DatabaseConnector
    {
        //Store Information of Currently SignedIn User
        public static User signeduser = new User();
        public static int myPostId = -1;
        //This function will check either the entered email is in correct format or not.
        public bool checkEmail(string email)
        {
            if (email[0] < 'a' || email[0] > 'z')
                return false;
            int indexOfAt = email.IndexOf('@', 0, email.Length - 1);
            if (indexOfAt == -1 || indexOfAt == 0)
                return false;
            int indexOfDot = -1;
            int countOfDot = 0, countOfAt = 0;
            for (int i = indexOfAt + 1; i < email.Length; i++)
            {
                if (email[i] == '.')
                {
                    countOfDot++;
                    indexOfDot = i;
                }
                else if (email[i] == '@')
                {
                    countOfAt++;
                }
            }
            if (countOfAt > 0)
                return false;
            if (countOfDot == 0)
                return false;
            if (indexOfDot < email.Length - 1)
                return true;
            return false;
        }
        //This function will check either the entered username is valid or not.
        public bool isUsernameValid(string s)
        {
            if (s.Length < 4)
                return false;
            int indexOfAt = s.IndexOf(' ', 0, s.Length - 1);
            if (indexOfAt == -1)
                return true;
            return false;
        }
        //This function will check either the entered email is already Exists or not.
        public bool isEmailExist(string email,int myid,bool func)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = "";
            SqlParameter p1 = new SqlParameter("e", email); ;
            SqlParameter p2 = new SqlParameter("i", myid);
            if (func)
                query = $"select * from Users " + $"where email = @e and Id != @i";
            else
                query = $"select * from Users " + $"where email = @e";
            SqlCommand cmd = new SqlCommand(query, con);
            if(func)
                cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            bool status = false;
            if (dr.HasRows)
            {
                status = true;
            }
            con.Close();
            return status;
        }
        //This function will check either the entered username is already exists or not.
        public bool isUsernameExist(string username, int myid, bool func)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = "";
            SqlParameter p1 = new SqlParameter("u", username); ;
            SqlParameter p2 = new SqlParameter("i", myid);
            if (func)
                query = $"select * from Users " + $"where username = @u and Id != @i";
            else
                query = $"select * from Users " + $"where username = @u";
            SqlCommand cmd = new SqlCommand(query, con);
            if (func)
                cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            bool status = false;
            if (dr.HasRows)
            {
                status = true;
            }
            con.Close();
            return status;
        }
        //This function will add User into the Users Record.
        public int AddUserInDBMS(User user)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"insert into Users (username,email,password,imagepath)" +
            $"  values(@u,@e,@p,@i)";
            SqlParameter p1 = new SqlParameter("u", user.username);
            SqlParameter p2 = new SqlParameter("e", user.email);
            SqlParameter p3 = new SqlParameter("p", user.password);
            SqlParameter p4 = new SqlParameter("i", user.imageName);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int status = cmd.ExecuteNonQuery();
            con.Close();
            return status;
        }
        //This will handle signedIn and check either user exists with given emails and password,
        //if exists then will return user's data with success status.
        public User handleSignIn(User user)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Users " +
                $"where email = @e and password= @p";
            SqlParameter p1 = new SqlParameter("e", user.email);
            SqlParameter p2 = new SqlParameter("p", user.password);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                if(dr.Read())
                {
                    user.id = int.Parse(dr["Id"].ToString());
                    user.username = dr["username"].ToString();
                    user.imageName = dr["imagepath"].ToString();
                }
            }
            con.Close();
            return user;
        }
        //This will add new Post into the Database.
        public void AddNewPost(Post post)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"insert into Posts (title,content,date,userId)" +
            $"  values(@t,@c,@d,@u)";
            SqlParameter p1 = new SqlParameter("t", post.title);
            SqlParameter p2 = new SqlParameter("c",post.content);
            SqlParameter p3 = new SqlParameter("d", post.date);
            SqlParameter p4 = new SqlParameter("u", post.userId);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            int status = cmd.ExecuteNonQuery();
            con.Close();
        }
        //This function will all posts of some Specfic User.
        public List<Post> getAllUsersPost(User user)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Posts " +
                $"where userId = @u";
            SqlParameter p1 = new SqlParameter("u", user.id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Post> postList = new List<Post>();
            if (dr.HasRows)
            {
                while(dr.Read())
                {
                    Post p = new Post();
                    p.Id = int.Parse(dr["Id"].ToString());
                    p.title = dr["title"].ToString();
                    p.content = dr["content"].ToString();
                    p.date = dr["date"].ToString();
                    p.userId = user.id;
                    p.username = user.username;
                    if (user.imageName == "")
                    {
                        p.imagepath = "~/ProfileImages/default.jpg";
                    }
                    else
                    {
                        string[] arr = user.imageName.Split(".");
                        p.imagepath = "~/ProfileImages/" + user.username + "." + arr[1];

                    }
                    postList.Add(p);
                }
            }
            con.Close();
            return postList;
        }
        //This will Delete Post form Database
        public void DeletePost(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"delete from Posts " +
                $"where Id = @i";
            SqlParameter p1 = new SqlParameter("i", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            int status = cmd.ExecuteNonQuery();
            con.Close();
        }
        //This will get some post data by using post ID.
        public NewPost GetPost(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Posts " +
                $"where Id = @u";
            SqlParameter p1 = new SqlParameter("u", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            NewPost p = new NewPost();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    p.title = dr["title"].ToString();
                    p.content = dr["content"].ToString();
                 }
            }
            con.Close();
            return p;
        }
        //This will Update the Post into the DBMS.
        public void UpdatePost(int id,NewPost p)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"update Posts set title = @t, content = @c " +
                $"where Id = @i";
            SqlParameter p1 = new SqlParameter("i", id);
            SqlParameter p2 = new SqlParameter("t", p.title);
            SqlParameter p3 = new SqlParameter("c", p.content);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            int status = cmd.ExecuteNonQuery();
            con.Close();
        }
        //This will return complete Data of some User whose ID match with given ID.
        public User getUser(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Users " +
                $"where Id = @i";
            SqlParameter p1 = new SqlParameter("i", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            User u = new User();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    u.id = int.Parse(dr["Id"].ToString());
                    u.username = dr["username"].ToString();
                    u.password = dr["password"].ToString();
                    u.email = dr["email"].ToString();
                    u.imageName = dr["imagepath"].ToString();
                }
            }
            con.Close();
            return u;
        }
        //This will Return All posts of all users from the Database.
        public List<Post> getAllPosts()
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Posts ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Post> postList = new List<Post>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Post p = new Post();
                    p.Id = int.Parse(dr["Id"].ToString());
                    p.title = dr["title"].ToString();
                    p.content = dr["content"].ToString();
                    p.date = dr["date"].ToString();
                    p.userId = int.Parse(dr["userId"].ToString());
                    User u = getUser(p.userId);
                    p.username = u.username;
                    if (u.imageName == "")
                    {
                        p.imagepath = "~/ProfileImages/default.jpg";
                    }
                    else
                    {
                        string extension = Path.GetExtension(u.imageName);
                        p.imagepath ="~/ProfileImages/" + u.username+extension;

                    }
                    postList.Add(p);
                }
            }
            con.Close();
            return postList;
        }
        //This will Resturn Complete data of some specific post whose id matches with given ID.
        public Post GetCompletePost(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Posts " +
                $"where Id = @u";
            SqlParameter p1 = new SqlParameter("u", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            SqlDataReader dr = cmd.ExecuteReader();
            Post p = new Post();
            if (dr.HasRows)
            {
                if (dr.Read())
                {
                    p.Id = int.Parse(dr["Id"].ToString());
                    p.title = dr["title"].ToString();
                    p.content = dr["content"].ToString();
                    p.date = dr["date"].ToString();
                    p.userId = int.Parse(dr["userId"].ToString());
                    User u = getUser(p.userId);
                    p.username = u.username;
                    if (u.imageName == "")
                    {
                        p.imagepath = "~/ProfileImages/default.jpg";
                    }
                    else
                    {
                        string[] arr = u.imageName.Split(".");
                        p.imagepath = "~/ProfileImages/" + u.username + "." + arr[1];

                    }
                }
            }
            con.Close();
            return p;
        }
        //This will Update the Given User into the Database.
        public void UpdateProfile(User user)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"update Users " + $"set username = @u, password= @p, email = @e, " +
                $"imagepath = @m where Id = @i";
            SqlParameter p1 = new SqlParameter("i", user.id);
            SqlParameter p2 = new SqlParameter("u", user.username);
            SqlParameter p3 = new SqlParameter("e", user.email);
            SqlParameter p4 = new SqlParameter("p", user.password);
            SqlParameter p5 = new SqlParameter("m", user.imageName);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            int status = cmd.ExecuteNonQuery();
            con.Close();
        }

        //This function will check either given data of Admin signedIn is correct or not.
        //Return True if correct otherwise return false.
        public bool isAdmin(AdminSignIn user)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Admin " +
                $"where username = @u and password= @p";
            SqlParameter p1 = new SqlParameter("u", user.username);
            SqlParameter p2 = new SqlParameter("p", user.password);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            SqlDataReader dr = cmd.ExecuteReader();
            bool status = false;
            if (dr.HasRows)
            {
                status = true;
            }
            con.Close();
            return status;
        }
        //This will return a list of All Users form database.
        public List<User> getAllUsers()
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"select * from Users"; 
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<User> UsersList = new List<User>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    User u = new User();
                    u.id = int.Parse(dr["Id"].ToString());
                    u.username = dr["username"].ToString();
                    u.password = dr["password"].ToString();
                    u.email = dr["email"].ToString();
                    u.imageName = dr["imagepath"].ToString();
                    if (u.imageName == "")
                    {
                        u.imageName = "~/ProfileImages/default.jpg";
                    }
                    else
                    {
                        string[] arr = u.imageName.Split(".");
                        u.imageName = "~/ProfileImages/" + u.username + "." + arr[1];

                    }
                    UsersList.Add(u);
                }
            }
            con.Close();
            return UsersList;
        }
        //This will Delete User from Database.
        public void DeleteUser(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"delete from Users " +
                $"where Id = @i";
            SqlParameter p1 = new SqlParameter("i", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            int status = cmd.ExecuteNonQuery();
            con.Close();
            DeleteAllPosts(id);
        }
        //This will delete all posts of the User whose id matches with given ID.
        public void DeleteAllPosts(int id)
        {
            string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=OnlineWebSite;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            string query = $"delete from Posts " +
                $"where userId = @i";
            SqlParameter p1 = new SqlParameter("i", id);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add(p1);
            int status = cmd.ExecuteNonQuery();
            con.Close();
        }

    }
}
