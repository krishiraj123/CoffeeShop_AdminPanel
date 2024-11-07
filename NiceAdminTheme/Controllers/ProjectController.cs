using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

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

        public IActionResult ProjectAddEdit(int ProjectID)
        {
            SqlCommand command = DatabaseConnection();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Projects_SelectByProjectID";
            command.Parameters.Add("ProjectID", SqlDbType.Int).Value = ProjectID;
            SqlDataReader dr = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(dr);
            ProjectModel projectModel = new ProjectModel();

            foreach (DataRow r in table.Rows)
            {
                projectModel.ProjectID = Convert.ToInt32(@r["ProjectID"]);
                projectModel.ProjectName = @r["ProjectName"].ToString();
                projectModel.Description = @r["Description"].ToString();
                projectModel.Status = @r["Status"].ToString();
                projectModel.Budget = Convert.ToInt32(@r["Budget"]);
                projectModel.CreatedDate = Convert.ToDateTime(@r["CreatedDate"]);
                projectModel.LastUpdated = Convert.ToDateTime(@r["LastUpdated"]);
            }
            
            return View("ProjectAddEdit", projectModel);
        }
        
        public IActionResult ProjectSave(ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {
                SqlCommand command = DatabaseConnection();
                command.CommandType = CommandType.StoredProcedure;
                if (projectModel.ProjectID == null)
                {
                    command.CommandText = "PR_Projects_Insert";
                }
                else
                {
                    command.CommandText = "PR_Projects_Update";
                    command.Parameters.Add("@ProjectID", SqlDbType.Int).Value = projectModel.ProjectID;
                }

                command.Parameters.Add("ProjectName", SqlDbType.VarChar).Value = projectModel.ProjectName;
                command.Parameters.Add("Description", SqlDbType.VarChar).Value = projectModel.Description;
                command.Parameters.Add("Status", SqlDbType.VarChar).Value = projectModel.Status;
                command.Parameters.Add("Budget", SqlDbType.Int).Value = projectModel.Budget;
                command.ExecuteNonQuery();
                return RedirectToAction("ListProjects");
            }
            return View("ProjectAddEdit", projectModel);
        }
    }
}
