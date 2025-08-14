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
        [HttpPost("select")]
        public async Task<IActionResult> Select(int id)
        {
            var unit = await _unitDataContext.GetByIdAsync(id);
            TempData["Id"] = unit.Id;
            TempData["Name"] = unit.Name;
            TempData["Status"] = unit.Status;

            return RedirectToAction("Edit");
        }
        [HttpGet("edit")]
        public IActionResult Edit()
        {
            var unit = new Unit
            {
                Id = Convert.ToInt32(TempData["Id"]),
                Name = TempData["Name"]?.ToString(),
                Status = Convert.ToInt32(TempData["Status"])
            };

            return View("Edit", unit);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> HandleEdit(Unit unit, string ActionType)
        {
            if (!ModelState.IsValid)
                return View("Edit", unit);

            switch (ActionType)
            {
                case "Update":
                    await _unitDataContext.UpdateAsync(unit);
                    break;
                case "Delete":
                    await _unitDataContext.DeleteAsync(unit);
                    break;
                case "Archive":
                    if (unit.Status == 1)
                    {
                        await _unitDataContext.ArchiveStatusAsync(unit);
                    }
                    else
                    {
                        await _unitDataContext.WorkStatusAsync(unit);
                    }
                    break;
                default:
                    return View("Edit", unit);
            }

            return RedirectToAction("Index", new { id = 1 });
        }
    }
}
