namespace StorageWeb.Repository.Receipt.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public required string ReceiptResource { get; set; }
        public required string ResourceUnit { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
