using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;

namespace NiceAdminTheme.Controllers;

public class StateController : Controller
{
    private IConfiguration config;

    public StateController(IConfiguration config)
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
    public IActionResult StateList()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_State_SelectAll";
        SqlDataReader reade = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reade);
        return View(dt);
    }
    #endregion

    #region SelectByID

    public IActionResult StateAddEdit(int? StateID)
    {
        SqlCommand cmd = Connection(); 
        StateModal sm = new StateModal();
        
        if (StateID.HasValue)
        {
            cmd.CommandText = "PR_State_SelectByID";
            cmd.Parameters.AddWithValue("StateID", StateID);
            SqlDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                sm.StateID = reader.GetInt32("StateID");
                sm.StateName = reader.GetString("StateName");
                sm.StateCode = reader.GetString("StateCode");
                sm.CountryID = reader.GetInt32("CountryID");
                sm.UserID = reader.GetInt32("UserID");
            }
        }
        UserDropdown();
        CountryDropdown();
        return View(sm);
    }
    #endregion

    #region Delete

    public IActionResult DeleteState(int StateID)
    {
        try
        {
            SqlCommand cmd = Connection();
            cmd.CommandText = "PR_State_Delete";
            cmd.Parameters.AddWithValue("StateID", StateID);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"]  = e.Message;
        }

        return RedirectToAction("StateList");
    }
    #endregion

    #region Insert

    public IActionResult StateSave(StateModal sm)
    {
        SqlCommand cmd = Connection();

        if (sm.StateID.HasValue)
        {
            cmd.CommandText = "PR_State_Update";
            cmd.Parameters.AddWithValue("StateID", sm.StateID);
        }
        else
        {
            cmd.CommandText = "PR_State_Insert";
        }

        cmd.Parameters.AddWithValue("StateName", sm.StateName);
        cmd.Parameters.AddWithValue("StateCode", sm.StateCode);
        cmd.Parameters.AddWithValue("CountryID", sm.CountryID);
        cmd.Parameters.AddWithValue("UserID", sm.UserID);

        cmd.ExecuteNonQuery();
        return RedirectToAction("StateList");
    }
    #endregion

    #region UserDropdown

    public void UserDropdown()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_User_DropDown";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);

        List<UserDropDownModel> user = new List<UserDropDownModel>();
        
        foreach (DataRow d in table.Rows)
        {
            UserDropDownModel u = new UserDropDownModel();
            u.UserID = Convert.ToInt32(d["UserID"]);
            u.UserName = d["UserName"].ToString();
            user.Add(u);
        }

        ViewBag.UserList = user;
    }
    #endregion

    #region CountryDropdown

    public void CountryDropdown()
    {
        SqlCommand cmd = Connection();
        cmd.CommandText = "PR_Country_Dropdown";
        SqlDataReader reader = cmd.ExecuteReader();
        DataTable table = new DataTable();
        table.Load(reader);

        List<CountryDropdownModel> cm = new List<CountryDropdownModel>();

        foreach (DataRow c in table.Rows)
        {
            CountryDropdownModel cu = new CountryDropdownModel();
            cu.CountryID = Convert.ToInt32(c["CountryID"]);
            cu.CountryName = c["CountryName"].ToString();
            cm.Add(cu);
        }

        ViewBag.CountryList = cm;
    }
    #endregion
}