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

		public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, List<string> receiptNumber, List<string> resource, List<string> unit)
		{
			var receipts = await _receiptDataContext.GetAsync();

			if (startDate.HasValue)
				receipts = receipts.Where(r => r.Date >= startDate.Value).ToList();

			if (endDate.HasValue)
				receipts = receipts.Where(r => r.Date <= endDate.Value).ToList();

			if (receiptNumber?.Any() == true)
				receipts = receipts.Where(r => receiptNumber.Contains(r.Number.ToString())).ToList();

			if (resource?.Any() == true)
				receipts = receipts.Where(r => resource.Contains(r.ReceiptResource)).ToList();

			if (unit?.Any() == true)
				receipts = receipts.Where(r => unit.Contains(r.ResourceUnit)).ToList();

			var numbers = await _receiptDataContext.GetReceiptNumbersAsync();
			var resources = await _receiptDataContext.GetResourcesAsync();
			var units = await _receiptDataContext.GetUnitsAsync();
			var firstDate = await _receiptDataContext.GetEarliestDateAsync();
			var lastDate = await _receiptDataContext.GetLatestDateAsync();

			var model = new ReceiptViewModel
			{
				Receipts = receipts,
				Numbers = numbers,
				Resources = resources,
				Units = units,
				FirstDate = firstDate,
				LastDate = lastDate,

				SelectedStartDate = startDate,
				SelectedEndDate = endDate,
				SelectedNumbers = receiptNumber ?? new List<string>(),
				SelectedResources = resource ?? new List<string>(),
				SelectedUnits = unit ?? new List<string>()
			};

			return View("Receipt", model);
		}
		[HttpGet("add")]
		public async Task<IActionResult> Add()
		{
			var model = new ReceiptAddViewModel
			{
				Date = DateTime.Today,
				ResourcesList = await _receiptDataContext.GetAllResourcesAsync(),
				UnitsList = await _receiptDataContext.GetAllUnitsAsync(),
			};
			return View("Add", model);
		}
		[HttpPost("add")]
		public async Task<IActionResult> Add(ReceiptAddViewModel model)
		{
			var receiptId = await _receiptDataContext.InsertReceiptAsync(new ReceiptAddViewModel
			{
				Number = model.Number,
				Date = model.Date
			});

			var resources = model.Resources.Select(r => new ReceiptResource
			{
				ReceiptId = receiptId,
				ResourceId = r.ResourceId,
				UnitId = r.UnitId,
				Count = r.Count
			});

			await _receiptDataContext.InsertReceiptResourcesAsync(resources);
			return RedirectToAction("Index");
		}
        [HttpPost("SetCurrentReceipt")]
        public IActionResult SetCurrentReceipt(int id)
        {
            TempData["CurrentReceiptId"] = id;
            TempData.Keep("CurrentReceiptId");
            return Ok();
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            if (TempData["CurrentReceiptId"] == null)
                return RedirectToAction("Index");

            int id = (int)TempData["CurrentReceiptId"];

            var receipt = await _receiptDataContext.GetByIdAsync(id);
            var resources = await _receiptDataContext.GetResourcesByReceiptIdAsync(id);
            var resourcesList = await _receiptDataContext.GetAllResourcesAsync();
            var unitsList = await _receiptDataContext.GetAllUnitsAsync();

            var model = new ReceiptAddViewModel
            {
                Number = receipt.Number,
                Date = receipt.Date,
                Resources = resources.ToList(),
                ResourcesList = resourcesList,
                UnitsList = unitsList
            };

            TempData.Keep("CurrentReceiptId");
            return View(model); // передаём в Edit.cshtml
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ReceiptAddViewModel model)
        {
            if (TempData["CurrentReceiptId"] == null)
                return RedirectToAction("Index");

            int id = (int)TempData["CurrentReceiptId"];

            await _receiptDataContext.UpdateReceiptAsync(id, model.Number, model.Date, model.Resources);

            TempData.Remove("CurrentReceiptId");
            return RedirectToAction("Index");
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete()
        {
            if (TempData["CurrentReceiptId"] == null)
                return RedirectToAction("Index");

            int id = (int)TempData["CurrentReceiptId"];

            try
            {
                await _receiptDataContext.DeleteReceiptAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Ошибка при удалении поступления");
            }
        }
    }
}
