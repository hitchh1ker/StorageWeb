using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Resource.Models;

namespace StorageWeb.Controllers
{
    [Route("/resources")]
    public class ResourceController : Controller
    {
        private readonly ResourceDataContext _resourceDataContext;

        public ResourceController(ResourceDataContext resourceDataContext)
        {
            _resourceDataContext = resourceDataContext;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            if (id == 1)
            {
                var resources = await _resourceDataContext.GetAsync(id);
                return View("Resource", resources);
            }
            else
            {
                var resources = await _resourceDataContext.GetAsync(id);
                return View("Archive", resources);
            }
        }
        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(Resource resource)
        {
            if (!ModelState.IsValid)
                return View("Add", resource);

            await _resourceDataContext.InsertAsync(resource);
            return RedirectToAction("Index", new { id = 1 });
        }
        [HttpPost("select")]
        public async Task<IActionResult> Select(int id)
        {
            var resource = await _resourceDataContext.GetByIdAsync(id);
            TempData["Id"] = resource.Id;
            TempData["Name"] = resource.Name;
            TempData["Status"] = resource.Status;
            return RedirectToAction("Edit");
        }
        [HttpGet("edit")]
        public IActionResult Edit()
        {
            var resource = new Resource
            {
                Id = Convert.ToInt32(TempData["Id"]),
                Name = TempData["Name"]?.ToString(),
                Status = Convert.ToInt32(TempData["Status"])
            };

            return View("Edit", resource);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> HandleEdit(Resource resource, string ActionType)
        {
            if (!ModelState.IsValid)
                return View("Edit", resource);

            switch (ActionType)
            {
                case "Update":
                    await _resourceDataContext.UpdateAsync(resource);
                    break;
                case "Delete":
                    await _resourceDataContext.DeleteAsync(resource);
                    break;
                case "Archive":
                    if (resource.Status == 1)
                    {
                        await _resourceDataContext.ArchiveStatusAsync(resource);
                    }
                    else
                    {
                        await _resourceDataContext.WorkStatusAsync(resource);
                    }
                    break;
                default:
                    return View("Edit", resource);
            }

            return RedirectToAction("Index", new { id = 1 });
        }
    }
}
