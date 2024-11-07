using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiceAdminTheme.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        [Required(ErrorMessage = "Select an Order ID")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Select an ProductID")]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Total Amount is required")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Select an UserID")]
        public int UserID { get; set; }
    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public String ProductName { get; set; }
    }
}
