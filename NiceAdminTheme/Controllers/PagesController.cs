using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

namespace NiceAdminTheme.Controllers
{    
    public class PagesController : Controller
    {
        #region Iconfigration
        private IConfiguration _config;

        public PagesController(IConfiguration configuration)
        {
            this._config = configuration;
        }
        #endregion Iconfigration
        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        #region UserRegister
        public IActionResult Register(UserRegisterModel userRegisterModel)
        {           
            return View(userRegisterModel);
        }

        public IActionResult UserSave(UserRegisterModel userRegisterModel)
        {
            try
            {
                string connectionString = this._config.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[PR_UserRegister_Insert]";
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                command.ExecuteNonQuery();
                return View("Login");
            }catch(Exception ex)
            {
                ViewBag.Error = "Username already exists";
                return View("Register");
            }
        }
        #endregion UserRegister

        #region PassLoginData
        public IActionResult Login(UserLogin userLogin)     
        {
            return View(userLogin);
        }
        #endregion PassLoginData

        #region Login
        public IActionResult LoginValidation(UserLogin userLogin)
        {
            string connectionString = this._config.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_User_Login";
                sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLogin.UserName;
                sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLogin.Password;

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {                    
                    var userId = reader["UserID"];
                    var UserName = reader["UserName"];
                    
                    HttpContext.Session.SetString("UserID", userId.ToString());
                    HttpContext.Session.SetString("UserName", UserName.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {                    
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View("Login");
                }
            }
        }
        #endregion Login

        #region SignOut
        public IActionResult SignOut()
        {
            var UserID = HttpContext.Session.GetString("UserID");
            if(!string.IsNullOrEmpty(UserID.ToString()))
            {
                HttpContext.Session.Clear();
            }
            else
            {
                ViewBag.Error = "User Not Found";
            }
            return View("Login");
        }
        #endregion SignOut
    }
}
