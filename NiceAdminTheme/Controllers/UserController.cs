using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;

namespace NiceAdminTheme.Controllers
{
    public class UserController : Controller
    {
        #region Iconfiguration
        private IConfiguration configuration;
        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion Iconfigration

        #region GetUserData
        private SqlDataReader GetUserData()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        #endregion GetUserData

        #region SelectAllUser
        public IActionResult UserList()
        {
            var reader = GetUserData();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion SelectAllUser

        #region UserAddEdit
        public IActionResult UserAddEdit(int UserID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            #region UserByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectByID";
            command.Parameters.AddWithValue("@UserID", UserID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            UserModel userModel = new UserModel();

            foreach (DataRow dataRow in table.Rows)
            {
                userModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                userModel.UserName = @dataRow["UserName"].ToString();
                userModel.Email = @dataRow["Email"].ToString();
                userModel.Password = @dataRow["Password"].ToString();
                userModel.MobileNo = @dataRow["MobileNo"].ToString();
                userModel.Address = @dataRow["Address"].ToString();
                userModel.IsActive = (Boolean)Convert.ToBoolean(dataRow["IsActive"]);
            }

            #endregion

            return View("UserAddEdit", userModel);
        }
        #endregion UserAddEdit

        #region UserSave
        public IActionResult Save(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("UserList");
            }
            else
            {
                return View("UserAddEdit", usermodel);
            }
        }
        #endregion UserSave

        #region UserDelete
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_Delete";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "Unable to delete the user as it has related records in other tables.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("UserList");
        }
        #endregion UserDelete

        #region UserAdd
        public IActionResult UserSave(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (userModel.UserID == null)
                {
                    command.CommandText = "PR_User_Insert";
                }
                else
                {
                    command.CommandText = "PR_User_Update";
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                }
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                command.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                command.ExecuteNonQuery();
                return RedirectToAction("Login");
            }

            return View("UserAddEdit", userModel);
        }
        #endregion UserAdd

        #region UserExportExcel
        [HttpGet]
        public async Task<IActionResult> UserExportExcel()
        {
            var reader = GetUserData();
            DataTable userList = new DataTable();
            userList.Load(reader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("userList");

                worksheet.Cells["A1"].LoadFromDataTable(userList, true);

                int columnCount = userList.Columns.Count;

                string rangeAddress = $"A1:{(char)('A' + columnCount - 1)}1";

                using (var range = worksheet.Cells[rangeAddress])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0;
                var fileName = "UserList.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion UserExportExcel
    }
}
