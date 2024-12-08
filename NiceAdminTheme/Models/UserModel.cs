using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models
{
    public class UserModel
    {
        public int? UserID { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "MobileNo is required")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public bool IsActive { get; set; }
    }

    public class CountryDropdownModel
    {
        public int CountryID { set; get; }
        public string CountryName { set; get; }
    }

    public class StateDropdownModel
    {
        public int StateID { set; get; }
        public string StateName { set; get; }
    }

    public class CityDropDownModel
    {
        public int CityID { set; get; }
        public string CityName { set; get; }
    }
}
