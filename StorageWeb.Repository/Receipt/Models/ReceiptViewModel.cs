namespace StorageWeb.Repository.Receipt.Models
{
    public class ReceiptViewModel
    {
        public IEnumerable<Receipt> Receipts { get; set; } = new List<Receipt>();
        public IEnumerable<int> Numbers { get; set; } = new List<int>();
        public IEnumerable<string> Resources { get; set; } = new List<string>();
        public IEnumerable<string> Units { get; set; } = new List<string>();
        public DateTime? FirstDate { get; set; }
        public DateTime? LastDate { get; set; }

		public DateTime? SelectedStartDate { get; set; }
		public DateTime? SelectedEndDate { get; set; }
		public List<string> SelectedNumbers { get; set; }
		public List<string> SelectedResources { get; set; }
		public List<string> SelectedUnits { get; set; }
	}
}
