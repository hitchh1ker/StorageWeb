using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Resource.Models;

namespace StorageWeb.Controllers
{
    [Route("/resources/{id}")]
    public class ResourceController : Controller
    {
        private readonly ResourceDataContext _resourceDataContext;

        public ResourceController(ResourceDataContext resourceDataContext)
        {
            _resourceDataContext = resourceDataContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var resources = await _resourceDataContext.GetAsync(id);
            return View("Resource", resources);
        }
    }
}
