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

        public async Task<IActionResult> Index()
        {
            var resources = await _resourceDataContext.GetAsync();
            return View("Resource", resources);
        }
    }
}
