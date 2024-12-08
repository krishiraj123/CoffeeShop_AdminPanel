using System.ComponentModel.DataAnnotations;

namespace NiceAdminTheme.Models;

public class StateModal
{
    public int? StateID { get; set; }
    [Required]
    public string StateName { get; set; }
    [Required]
    public string StateCode { get; set; }
    [Required]
    public int CountryID { get; set; }
    [Required]
    public int UserID { get; set; }
}