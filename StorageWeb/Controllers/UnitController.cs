using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Unit.Models;

namespace StorageWeb.Controllers
{
    [Route("units")]
    public class UnitController : Controller
    {
        private readonly UnitDataContext _unitDataContext;

        public UnitController(UnitDataContext unitDataContext)
        {
            _unitDataContext = unitDataContext;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            if (id == 1)
            {
                var units = await _unitDataContext.GetAsync(id);
                return View("Unit", units);
            }
            else
            {
                var units = await _unitDataContext.GetAsync(id);
                return View("Archive", units);
            }
        }
        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(Unit unit)
        {
            if (!ModelState.IsValid)
                return View("Add", unit);

            await _unitDataContext.InsertAsync(unit);
            return RedirectToAction("Index", new { id = 1 });
        }
    }
}
