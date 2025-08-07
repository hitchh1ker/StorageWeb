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
        [HttpGet("edit")]
        public IActionResult Edit()
        {
            return View("Edit");
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Resource resource)
        {
            if (!ModelState.IsValid)
                return View("Add", resource);
            await _resourceDataContext.DeleteAsync(resource);
            return RedirectToAction("Index", new { id = 1 });
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(Resource resource)
        {
            if (!ModelState.IsValid)
                return View("Add", resource);
            await _resourceDataContext.UpdateAsync(resource);
            return RedirectToAction("Index", new { id = 1 });
        }
        [HttpPut("inarchive")]
        public async Task<IActionResult> InArchive(Resource resource)
        {
            if (!ModelState.IsValid)
                return View("Add", resource);
            await _resourceDataContext.UpdateStatusAsync(resource);
            return RedirectToAction("Index", new { id = 1 });
        }
    }
}
