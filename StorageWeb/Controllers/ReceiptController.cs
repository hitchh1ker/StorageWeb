using Microsoft.AspNetCore.Mvc;
using StorageWeb.Repository.Receipt.Models;

namespace StorageWeb.Controllers
{
    [Route("/receipts")]
    public class ReceiptController : Controller
    {
        private readonly ReceiptDataContext _receiptDataContext;

        public ReceiptController(ReceiptDataContext receiptDataContext)
        {
            _receiptDataContext = receiptDataContext;
        }

        public async Task<IActionResult> Index()
        {
            var receipts = await _receiptDataContext.GetAsync();
            return View("Receipt", receipts);
        }
    }
}
