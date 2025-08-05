using Microsoft.AspNetCore.Mvc;

namespace StorageWeb.Controllers
{
    public class UnitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
