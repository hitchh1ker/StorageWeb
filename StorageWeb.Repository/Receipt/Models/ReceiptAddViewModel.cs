namespace StorageWeb.Repository.Receipt.Models
{
	public class ReceiptAddViewModel
	{
		public int Number { get; set; }
		public DateTime Date { get; set; } = DateTime.Today;
		public List<ReceiptResource> Resources { get; set; } = new();

		public IEnumerable<ResourceTable> ResourcesList { get; set; }
		public IEnumerable<UnitTable> UnitsList { get; set; }
	}
}
