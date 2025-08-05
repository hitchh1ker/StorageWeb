using Microsoft.Extensions.Options;

namespace StorageWeb.Repository.Receipt
{
    public class ReceiptDataContext
    {
        private readonly string ConnectionString;

        public ReceiptDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
    }
}
