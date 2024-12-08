using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class TableController : Controller
    {
        public IActionResult GeneralTable()
        {
            return View();
        }
        public IActionResult DataTable()
        {
            return View();
        }
    }
}
