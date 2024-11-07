using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.Drawing;

namespace NiceAdminTheme.Controllers
{
    public class BillsController : Controller
    {
        #region Iconfiguration
        private IConfiguration configuration;
        public BillsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion Iconfiguration

        #region GetBillData
        private SqlDataReader GetBillData()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        #endregion GetBillData

        #region SelectAllBills
        public IActionResult ListBills()
        {
            var reader = GetBillData();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion SelectAllBills

        #region SelectBillByID
        public IActionResult BillsAddEdit(int BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectByID";
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillsModel billModel = new BillsModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billModel.BillID = Convert.ToInt32(@dataRow["BillID"]);
                billModel.BillNumber = @dataRow["BillNumber"].ToString();
                billModel.BillDate =Convert.ToDateTime( @dataRow["BillDate"]);
                billModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                billModel.Discount = Convert.ToDecimal(@dataRow["Discount"]);
                billModel.NetAmount = Convert.ToDecimal(@dataRow["NetAmount"]);
                billModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            OrderDropDown();
            UserDropDown();
            return View("BillsAddEdit", billModel);
            
        }
        #endregion SelectBillByID

        #region OrderDropDown
        public void OrderDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderList.Add(orderDropDownModel);
            }            
            ViewBag.OrderList = orderList;
        }
        #endregion OrderDropDown

        #region UserDropDown
        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;
        }
        #endregion UserDropDown

        #region BillsSave
        public IActionResult Save(BillsModel billsmodel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListBills");
            }
            else
            {
                return View("BillsAddEdit", billsmodel);
            }
        }
        #endregion BillsSave

        #region BillsDelete
        public IActionResult BillsDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_Delete";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "Unable to delete the bill as it has related records in other tables.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("ListBills");
        }
        #endregion BillsDelete

        #region BillsAdd
        public IActionResult BillsSave(BillsModel billsModel)
        {
            if (billsModel.BillID  <= 0)
            {
                ModelState.AddModelError("BillID", "A valid BillID is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (billsModel.BillID == null)
                {
                    command.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    command.CommandText = "PR_Bills_Update";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                }
                command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("ListBills");
            }
            return View("BillsAddEdit", billsModel);
        }
        #endregion BillsAdd

        #region BillExportExcel
        [HttpGet]
        public async Task<IActionResult> BillExportExcel()
        {
            var reader = GetBillData();
            DataTable orderList = new DataTable();
            orderList.Load(reader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("userList");

                worksheet.Cells["A1"].LoadFromDataTable(orderList, true);

                int columnCount = orderList.Columns.Count;

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
                var fileName = "BillList.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion BillExportExcel
    }
}
