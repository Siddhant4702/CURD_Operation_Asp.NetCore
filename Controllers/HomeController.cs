using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CURD_using_SDSclient.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CURD_using_SDSclient.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

       
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //String query = "INSERT INTO Users(FirstName, LastName, Email)VALUES(@FirstName, @LastName, @Email)";
                using (SqlCommand command = new SqlCommand("AddNewUser", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("List");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            List<User> allUser = new List<User>();
            using(SqlConnection connection =new SqlConnection(_connectionString))
            {
                connection.Open();
              //  String query = "SELECT Id, FirstName, LastName, Email FROM USERS";
                using (SqlCommand command=new SqlCommand("getAllUsers", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            User newUser = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName  =reader["LastName"].ToString(),
                                Email     =reader["Email"].ToString()   
                            };
                            allUser.Add(newUser);
                        }
                    }
                    
                }
            }
            return View(allUser);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            User userToUpdate;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
               // string query = "SELECT Id, FirstName, LastName, Email FROM Users WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand("getUserById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userToUpdate = new User
                            {
                                Id = (int)reader["Id"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                        else
                        {
                            return NotFound(); // If user with provided id is not found
                        }
                    }
                }
            }
            return View(userToUpdate);
        }

        [HttpPost]
        public IActionResult Update(User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand("UpdateUserById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedUser.LastName);
                    command.Parameters.AddWithValue("@Email", updatedUser.Email);
                    command.Parameters.AddWithValue("@Id", updatedUser.Id);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
              //  string query = "DELETE FROM Users WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand("DeleteById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("List");
        }


    }
}
