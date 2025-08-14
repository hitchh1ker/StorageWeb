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

            var sql = $"SELECT r.id AS Id, r.number AS Number, rres.count AS Count, r.date AS Date, res.name AS ReceiptResource, u.name AS ResourceUnit FROM receipt r LEFT JOIN" +
                $" receipt_resource rres ON r.id = rres.receipt_id LEFT JOIN resource res ON rres.resource_id = res.id LEFT JOIN unit u ON rres.unit_id = u.id ORDER BY r.date, r.number;";

            var receipts = await connection.QueryAsync<Receipt>(sql);
            return receipts;
        }
        public async Task<IEnumerable<int>> GetReceiptNumbersAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<int>("SELECT DISTINCT number FROM receipt ORDER BY number");
        }

        public async Task<IEnumerable<string>> GetResourcesAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<string>("SELECT DISTINCT name FROM resource where status = @Status ORDER BY name" , new { Status = 1 });
        }

        public async Task<IEnumerable<string>> GetUnitsAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<string>("SELECT DISTINCT name FROM unit where status = @Status ORDER BY name" , new { Status = 1 });
        }
        public async Task<DateTime?> GetEarliestDateAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<DateTime?>(
                "SELECT MIN(date) FROM receipt"
            );
        }

        public async Task<DateTime?> GetLatestDateAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<DateTime?>(
                "SELECT MAX(date) FROM receipt"
            );
        }

    }
}
