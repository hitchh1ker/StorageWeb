using Microsoft.AspNetCore.Mvc;

namespace StorageWeb.Controllers
{
    public class ResourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
