using Microsoft.AspNetCore.Mvc;

namespace NiceAdminTheme.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult Chart()
        {
            return View();
        }
        public IActionResult ApexChart()
        {
            return View();
        }
        public IActionResult EChart()
        {
            return View();
        }
    }
}
