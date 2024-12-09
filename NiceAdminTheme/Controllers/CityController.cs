using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

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

    #region SelectByID

    public IActionResult CityAddEdit(int? CityID)
    {
        SqlCommand cmd = Connection();
        CityModel cm = new CityModel();
        
        if (CityID.HasValue)
        {
            cmd.CommandText = "PR_City_SelectByID";
            cmd.Parameters.AddWithValue("CityID", CityID);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                cm.CityID = reader.GetInt32("CityID");
                cm.CityName = reader.GetString("CityName");
                cm.Pincode = reader.GetString("Pincode");
                cm.StateID = reader.GetInt32("StateID");
                cm.UserID = reader.GetInt32("UserID");
            }
        }
        StateDropdown();
        UserDropdown();
        return View(cm);
    }
    #endregion

    #region Delete

    public IActionResult DeleteCity(int CityID)
    {
        try
        {
            SqlCommand cmd = Connection();
            cmd.CommandText = "PR_City_Delete";
            cmd.Parameters.AddWithValue("CityID", CityID);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToAction("CityList");
    }
    #endregion

    #region Insert/Update

    public IActionResult CitySave(CityModel model)
    {
        SqlCommand cmd = Connection();
        if (ModelState.IsValid)
        {
            if (model.CityID.HasValue)
            {
                cmd.CommandText = "PR_City_Update";
                cmd.Parameters.AddWithValue("CityID", model.CityID);
            }
            else
            {
                cmd.CommandText = "PR_City_Insert";
            }

            cmd.Parameters.AddWithValue("CityName", model.CityName);
            cmd.Parameters.AddWithValue("Pincode", model.Pincode);
            cmd.Parameters.AddWithValue("StateID", model.StateID);
            cmd.Parameters.AddWithValue("UserID", model.UserID);
            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("CityList");
    }
    #endregion

    #region StateDropdown

    public void StateDropdown()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_State_Dropdown";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        List<StateDropdownModel> sm = new List<StateDropdownModel>();

        foreach (DataRow s in dt.Rows)
        {
            StateDropdownModel model = new StateDropdownModel();
            model.StateID = Convert.ToInt32(s["StateID"]);
            model.StateName = s["StateName"].ToString();
            sm.Add(model);
        }

        ViewBag.StateList = sm;
    }
    #endregion

    #region UserDropdown

    public void UserDropdown()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_User_DropDown";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        List<UserDropDownModel> sm = new List<UserDropDownModel>();

        foreach (DataRow s in dt.Rows)
        {
            UserDropDownModel model = new UserDropDownModel();
            model.UserID = Convert.ToInt32(s["UserID"]);
            model.UserName = s["UserName"].ToString();
            sm.Add(model);
        }

        ViewBag.UserList = sm;
    }
    #endregion
}