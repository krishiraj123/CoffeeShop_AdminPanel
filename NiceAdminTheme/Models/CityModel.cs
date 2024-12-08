using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models;

public class CityModel
{
    public int? CityID { get; set; }
    [Required]
    public string CityName { get; set; }
    [Required]
    public string Pincode { get; set; }
    [Required]
    public int StateID { get; set; }
    [Required]
    public int UserID { get; set; }
}