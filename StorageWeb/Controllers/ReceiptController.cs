using Microsoft.AspNetCore.Mvc;

namespace StorageWeb.Controllers
{
    public class ReceiptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
