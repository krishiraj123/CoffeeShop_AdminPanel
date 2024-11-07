using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
