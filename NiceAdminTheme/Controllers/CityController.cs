using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers;

public class CityController : Controller
{
    private IConfiguration config;

    public CityController(IConfiguration config)
    {
        this.config = config;
    }

    #region Connection

    public SqlCommand Connection()
    {
        SqlConnection cons = new SqlConnection(this.config.GetConnectionString("ConnectionString"));
        cons.Open();
        SqlCommand cmd = cons.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        return cmd;
    }
    #endregion

    #region SelectAll

    public IActionResult CityList()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_City_SelectAll";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        return View(dt);
    }
    #endregion
}