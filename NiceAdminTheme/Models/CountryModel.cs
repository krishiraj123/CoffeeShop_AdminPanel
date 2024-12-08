using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models;

public class CountryModel
{ 
    public int? CountryID { get; set; }
    [Required]
    public string CountryName { get; set; }
    [Required]
    public int UserID { get; set; }
}
