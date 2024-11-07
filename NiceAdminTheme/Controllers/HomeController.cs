using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace NiceAdminTheme.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index(OrderModel orderModel)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_DashBoardDetails";
            SqlDataReader reader = command.ExecuteReader();
            DataTable tb = new DataTable();
            tb.Load(reader);
            Console.WriteLine(tb);

            return View(tb);
        }
    }
}
