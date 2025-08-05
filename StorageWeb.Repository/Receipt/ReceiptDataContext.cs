using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace StorageWeb.Repository.Receipt.Models
{
    public class ReceiptDataContext
    {
        private readonly string ConnectionString;

        public ReceiptDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
        public async Task<IEnumerable<Receipt>> GetAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var receipts = await connection.QueryAsync<Receipt>($"select r.id as Id, r.number as Number, rres.count as Count, date, res.name " +
                $"as ReceiptResource, u.name as ResourceUnit from receipt r join receipt_resource rres on r.id = rres.receipt_id join " +
                $"resource res on rres.resource_id = res.id join unit u on rres.unit_id = u.id group by r.id, r.number, rres.count, r.date, res.name, u.name;");
            return receipts;
        }
    }
}
