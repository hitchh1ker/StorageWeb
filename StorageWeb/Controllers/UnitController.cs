using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Unit.Models;

namespace StorageWeb.Controllers
{
    [Route("/units")]
    public class UnitController : Controller
    {
        private readonly UnitDataContext _unitDataContext;

        public UnitController(UnitDataContext unitDataContext)
        {
            _unitDataContext = unitDataContext;
        }

        public async Task<IActionResult> Index()
        {
            var units = await _unitDataContext.GetAsync();
            return View("Unit", units);
        }
    }
}
