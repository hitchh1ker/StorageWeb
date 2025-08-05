using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Unit.Models;

namespace StorageWeb.Controllers
{
    [Route("/units/{id}")]
    public class UnitController : Controller
    {
        private readonly UnitDataContext _unitDataContext;

        public UnitController(UnitDataContext unitDataContext)
        {
            _unitDataContext = unitDataContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var units = await _unitDataContext.GetAsync(id);
            return View("Unit", units);
        }
    }
}
