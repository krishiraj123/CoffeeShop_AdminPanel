using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
/*using AspNetCore;*/

namespace NiceAdminTheme.Controllers
{
    public class OrderController : Controller
    {
        #region Iconfiguration
        private IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion Iconfiguration

        #region GetOrderData           
        private SqlDataReader GetOrderData()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            return reader;              
        }
        #endregion GetOrderData

        #region SelectAllOrders
        public IActionResult OrderList()
        {
            var reader = GetOrderData();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion SelectAllOrders

        #region OrderAddEdit
        public IActionResult OrderAddEdit(int OrderID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            #region OrderByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectByID";
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderModel orderModel = new OrderModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@dataRow["OrderDate"]);
                orderModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                orderModel.PaymentMode =@dataRow["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderModel.ShippingAddress = @dataRow["ShippingAddress"].ToString();
                orderModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion
            UserDropDown();
            CustomerDropDown();
            return View("OrderAddEdit", orderModel);            
        }
        #endregion OrderAddEdit

        #region UserDropDown
        public void UserDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
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

        #region CustomerDropDown
        public IActionResult CustomerDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(data["CustomerID"]);
                customerDropDownModel.CustomerName = data["CustomerName"].ToString();
                customerList.Add(customerDropDownModel);
            }
            ViewBag.CustomerList = customerList;
            return View();
        }
        #endregion CustomerDropDown

        #region OrderSave
        public IActionResult Save(OrderModel ordermodel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderList");
            }
            else
            {
                UserDropDown();
                CustomerDropDown();
                return View("OrderAddEdit", ordermodel);
            }
        }
        #endregion OrderSave

        #region OrderDelete
        public IActionResult OrderDelete(int OrderID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_Delete";
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 547) 
            {
                TempData["ErrorMessage"] = "Unable to delete the order as it has related records in other tables.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("OrderList");
        }
        #endregion OrderDelete

        #region OrderAdd
        public IActionResult OrderSave(OrderModel orderModel)
        {
            if (orderModel.UserID <= 0 && orderModel.CustomerID <= 0)
            {
                ModelState.AddModelError("UserID, CustomerID", "A valid User and Customer is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (orderModel.OrderID == null)
                {
                    command.CommandText = "PR_Order_Insert";
                }
                else
                {
                    command.CommandText = "PR_Order_Update";
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;
                }
                command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = orderModel.CustomerID;
                command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
                command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderList");
            }

            return View("OrderAddEdit", orderModel);
        }
        #endregion OrderAdd

        #region OrderExportExcel
        [HttpGet]
        public async Task<IActionResult> OrderExportExcel()
        {
            var reader = GetOrderData();
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
                var fileName = "OrderList.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion OrderExportExcel
    }
}
