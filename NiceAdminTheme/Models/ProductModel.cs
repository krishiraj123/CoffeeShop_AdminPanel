using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Price is required")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "Product Code is required")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Product Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "User id is required")]
        public int UserID { get; set; }
    }
}
