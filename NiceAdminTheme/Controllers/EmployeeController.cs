using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult ListEmployees()
        {
            return View();
        }
        public IActionResult InsertEmployee()
        {
            return View();
        }
        public IActionResult EditEmployee()
        {
            return View();
        }
    }
}
