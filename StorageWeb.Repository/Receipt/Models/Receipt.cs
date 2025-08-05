namespace StorageWeb.Repository.Receipt.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string ReceiptResource { get; set; }
        public string ResourceUnit { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
