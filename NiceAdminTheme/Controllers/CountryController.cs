using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

namespace NiceAdminTheme.Controllers;

public class CountryController : Controller
{
    private IConfiguration config;

    public CountryController(IConfiguration config)
    {
        this.config = config;
    }

    public SqlCommand Connection()
    {
        SqlConnection cons = new SqlConnection(this.config.GetConnectionString("ConnectionString"));
        cons.Open();
        SqlCommand cmd = cons.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        return cmd;
    }

    #region SelectAll

    public IActionResult CountryList()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_Country_SelectAll";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        return View(dt);
    }

    #endregion

    #region SelectByID

    public IActionResult CountryAddEdit(int? CountryID)
    {
        SqlCommand cmd = Connection();
        CountryModel cm = new CountryModel();
        if (CountryID.HasValue)
        {
            cmd.CommandText = "PR_Country_SelectByID";
            cmd.Parameters.AddWithValue("CountryID", CountryID);
            SqlDataReader reader = cmd.ExecuteReader();
            
            if(reader.Read())
            {
                cm.CountryID = reader.GetInt32("CountryID");
                cm.CountryName = reader.GetString("CountryName");
                cm.UserID = reader.GetInt32("UserID");
            }
        }
        UserDropdown();
        return View(cm);
    }
    #endregion

    #region Delete

    public IActionResult DeleteCountry(int CountryID)
    {
        try
        {
            SqlCommand cmd = Connection();
            cmd.CommandText = "PR_Country_Delete";
            cmd.Parameters.Add("CountryID", SqlDbType.Int).Value = CountryID;
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex) when (ex.Number == 547)
        {
            TempData["ErrorMessage"] = "Unable to delete the user as it has related records in other tables.";
        }

        return RedirectToAction("CountryList");
    }

    #endregion

    #region Insert

    public IActionResult CountrySave(CountryModel model)
    {
        SqlCommand cmd = Connection();

        if (model.CountryID.HasValue)
        {
            cmd.CommandText = "PR_Country_Update";
            cmd.Parameters.AddWithValue("CountryID", model.CountryID.Value);
        }
        else
        {
            cmd.CommandText = "PR_Country_Insert";
        }

        cmd.Parameters.AddWithValue("CountryName", model.CountryName);
        cmd.Parameters.AddWithValue("UserID", model.UserID);

        cmd.ExecuteNonQuery();
        return RedirectToAction("CountryList");
    }
    #endregion

    #region UserDropdown

    public void UserDropdown()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_User_DropDown";
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(dr);
        List<UserDropDownModel> u = new List<UserDropDownModel>();

        foreach (DataRow d in dt.Rows)
        {
            UserDropDownModel ur = new UserDropDownModel();
            ur.UserID = Convert.ToInt32(d["UserID"]);
            ur.UserName = d["UserName"].ToString();
            u.Add(ur);
        }

        ViewBag.UserList = u;
    }
    #endregion
}