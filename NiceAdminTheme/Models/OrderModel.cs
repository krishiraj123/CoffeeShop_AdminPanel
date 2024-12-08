using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NiceAdminTheme.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }
        [Required(ErrorMessage = "OrderDate is required")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "Select a CustomerID")]
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "PaymentMode is required")]
        public string PaymentMode { get; set; }
        [Required(ErrorMessage = "TotalAmount is required")]
        public decimal? TotalAmount { get; set; }
        [Required(ErrorMessage = "ShippingAddress is required")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "Select a UserID")]
        public int UserID { get; set; }
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }

}
