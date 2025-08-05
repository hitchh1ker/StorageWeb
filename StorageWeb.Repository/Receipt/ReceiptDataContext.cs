using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

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
