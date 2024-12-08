using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace NiceAdminTheme.Models;

public class ProjectModel
{
    [Required] public int ProjectID { get; set; }
    [Required] public string ProjectName { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Status { get; set; }
    [Required] public int Budget { get; set; }
    [Required] public DateTime CreatedDate { get; set; }
    [Required] public DateTime LastUpdated { get; set; }
}