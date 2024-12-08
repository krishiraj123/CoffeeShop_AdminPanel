using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult ListDepartments()
        {
            return View();
        }
        public IActionResult InsertDepartment()
        {
            return View();
        }
        public IActionResult UpdateDepartment()
        {
            return View();
        }
    }
}
