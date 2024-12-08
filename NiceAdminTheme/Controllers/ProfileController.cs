using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Runtime.CompilerServices;

namespace NiceAdminTheme.Controllers
{
    public class ProfileController : Controller
    {
        private IConfiguration _config;

        public ProfileController(IConfiguration configuration)
        {
            this._config = configuration;
        }

        public IActionResult Profile()
        {
            string userId = HttpContext.Session.GetString("UserID");
            string connectionString = this._config.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_UserRegister_SelectByID";
                command.Parameters.AddWithValue("@UserID", userId);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var user = new UserModel
                    {
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),                       
                    };

                    return View(user);
                }
                else
                {                   
                    return RedirectToAction("Error","Error");
                }
            }
        }
    }
}
