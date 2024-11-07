using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }
        [Required(ErrorMessage = "Customer name is required")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "HomeAddress is required")]
        public string HomeAddress { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "MobileNo is required")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "GSTNO is required")]
        public string GSTNO { get; set; }
        [Required(ErrorMessage = "CityName is required")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "PinCode is required")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "NetAmount is required")]
        public decimal NetAmount { get; set; }
        [Required(ErrorMessage = "Select an User ID")]
        public int UserID { get; set; }
    }
}
