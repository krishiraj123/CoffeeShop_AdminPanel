using Microsoft.AspNetCore.Mvc;
using NiceAdminTheme.Models;
using System.Data.SqlClient;
using System.Data;
using System;
using OfficeOpenXml;

namespace NiceAdminTheme.Controllers
{
    public class OrderDetailController : Controller
    {
        #region Iconfiguration
        private IConfiguration _configuration;
        public OrderDetailController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        #endregion Iconfiguration

        #region GetOrderDetailData
        private SqlDataReader GetOrderDetailData()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        #endregion GetOrderDetailData

        #region SelectAllOrderDetails
        public IActionResult ListOrderDetail()
        {   
            var reader = GetOrderDetailData();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion SelectAllOrderDetails

        #region OrderDetailAddEdit
        public IActionResult OrderDetailAddEdit(int OrderDetailID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            #region OrderDetailByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectByID";
            command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderDetailModel orderDetailModel = new OrderDetailModel();

            foreach (DataRow dataRow in table.Rows)
            {
                orderDetailModel.OrderDetailID = Convert.ToInt32(@dataRow["OrderDetailID"]);
                orderDetailModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                orderDetailModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                orderDetailModel.Quantity = Convert.ToInt32(@dataRow["Quantity"]);
                orderDetailModel.Amount = Convert.ToDecimal(@dataRow["Amount"]);
                orderDetailModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderDetailModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion
            UserDropDown();
            ProductDropDown();
            OrderDropDown();
            return View("OrderDetailAddEdit", orderDetailModel);
        }
        #endregion OrderDetailAddEdit

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

        #region ProductDropDown
        public void ProductDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Product_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<ProductDropDownModel> productList = new List<ProductDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(data["ProductID"]);
                productDropDownModel.ProductName = data["ProductName"].ToString();
                productList.Add(productDropDownModel);
            }
            ViewBag.ProductList = productList;
        }
        #endregion ProductDropDown

        #region OrderDropDown
        public void OrderDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
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

        #region OrderSave
        public IActionResult Save(OrderDetailModel orderdetailmodel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("ListOrderDetail");
            }
            else
            {
                return View("OrderDetailAddEdit", orderdetailmodel);
            }
        }
        #endregion OrderSave

        #region OrderDetailDelete
        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_Delete";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                TempData["ErrorMessage"] = "Unable to delete the orderdetail as it has related records in other tables.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("ListOrderDetail");
        }
        #endregion OrderDetailDelete

        #region OrderDetailsAdd
        public IActionResult OrderSave(OrderDetailModel orderDetailModel)
        {
            if (orderDetailModel.OrderID <= 0)
            {
                ModelState.AddModelError("OrderID", "A valid Order is required.");
            }
            if (orderDetailModel.ProductID <= 0)
            {
                ModelState.AddModelError("ProductID", "A valid Product is required.");
            }
            if (orderDetailModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (!ModelState.IsValid)
            {
                UserDropDown();
                ProductDropDown();
                OrderDropDown();

                return View("OrderDetailAddEdit", orderDetailModel);
            }

            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            if (orderDetailModel.OrderDetailID == null)
            {
                command.CommandText = "PR_OrderDetail_Insert";
            }
            else
            {
                command.CommandText = "PR_OrderDetail_Update";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailModel.OrderDetailID;
            }
            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderDetailModel.OrderID;
            command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderDetailModel.ProductID;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderDetailModel.Quantity;
            command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderDetailModel.Amount;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderDetailModel.TotalAmount;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderDetailModel.UserID;
            command.ExecuteNonQuery();

            return RedirectToAction("ListOrderDetail");
        }
        #endregion OrderDetailsAdd

        #region OrderDetailExportExcel
        [HttpGet]
        public async Task<IActionResult> OrderDetailExportExcel()
        {
            var reader = GetOrderDetailData();
            DataTable List = new DataTable();
            List.Load(reader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("userList");

                worksheet.Cells["A1"].LoadFromDataTable(List, true);

                int columnCount = List.Columns.Count;

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
                var fileName = "OrderDetailList.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion OrderDetailExportExcel
    }
}
