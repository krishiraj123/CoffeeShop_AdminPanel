using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

namespace NiceAdminTheme.Controllers
{
    public class UserRegisterController : Controller
    {
        private IConfiguration _configuration;

        public UserRegisterController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult UserRegister(UserRegisterModel registerModel)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_UserRegister_Insert";
                sqlCommand.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar).Value = registerModel.UserName;
                sqlCommand.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = registerModel.Email;
                sqlCommand.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = registerModel.Password;

                sqlCommand.ExecuteNonQuery();
                return View();
            }catch(Exception ex)
            {
                ViewBag.Error = "Please User Different UserName";
                return View("Pages","Register");
            }
        }
    }
}
