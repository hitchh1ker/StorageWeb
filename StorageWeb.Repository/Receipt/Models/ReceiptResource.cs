using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageWeb.Repository.Receipt.Models
{
    public class ReceiptResource
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int ReceiptId { get; set; }
        public int UnitId { get; set; }
        public int Count { get; set; }
    }
}
