using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models
{
    public class BillsModel
    {
        public int? BillID { get; set; }
        [Required(ErrorMessage = "BillNumber is required")]
        public string BillNumber { get; set; }
        [Required(ErrorMessage = "BillDate is required")]
        public DateTime BillDate { get; set; }
        [Required(ErrorMessage = "Select an OrderID")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "TotalAmount is required")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Discount is required")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "NetAmount is required")]
        public decimal NetAmount { get; set; }
        [Required(ErrorMessage = "Select an UserID")]
        public int UserID { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
    }
}
