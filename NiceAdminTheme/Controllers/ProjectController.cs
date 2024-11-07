using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class ProjectController : Controller
    {
        private IConfiguration _configuration;

        public ProjectController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public SqlCommand DatabaseConnection()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString")!;
            SqlConnection cons = new SqlConnection(connectionString);
            cons.Open();
            SqlCommand command = cons.CreateCommand();
            return command;
        }
        
        public IActionResult ListProjects()
        {
            SqlCommand command = DatabaseConnection();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Projects_SelectAll";
            SqlDataReader dr = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(dr);
            
            return View(table);
        }

        public IActionResult ProjectDelete(int ProjectID)
        {
            try
            {
                SqlCommand command = DatabaseConnection();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Projects_Delete";
                command.Parameters.Add("ProjectID", SqlDbType.Int).Value = ProjectID;
                command.ExecuteNonQuery();
                return View("ListProjects");
            }
            catch (SqlException e) when (e.Number == 547)
            {
                @TempData["Error"] = "This project is being used in other tables";
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred: " + e.Message;
            }
            return RedirectToAction("ListProjects");
        }
        
        public IActionResult ProjectAddEdit()
        {
            return View();
        }
    }
}
