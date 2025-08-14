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
            var model = new ReceiptViewModel
            {
                Numbers = await _receiptDataContext.GetReceiptNumbersAsync(),
                Resources = await _receiptDataContext.GetResourcesAsync(),
                Units = await _receiptDataContext.GetUnitsAsync(),
                FirstDate = await _receiptDataContext.GetEarliestDateAsync(),
                LastDate = await _receiptDataContext.GetLatestDateAsync(),
                Receipts = await _receiptDataContext.GetAsync()
            };

            return View("Receipt", model);
        }
    }
}
