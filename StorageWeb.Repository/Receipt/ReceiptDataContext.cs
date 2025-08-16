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
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            var sql = $"SELECT r.id AS Id, r.number AS Number, rres.count AS Count, r.date AS Date, res.name AS ReceiptResource, u.name AS ResourceUnit FROM receipt r LEFT JOIN" +
                $" receipt_resource rres ON r.id = rres.receipt_id LEFT JOIN resource res ON rres.resource_id = res.id LEFT JOIN unit u ON rres.unit_id = u.id ORDER BY r.date, r.number;";

            var receipts = await connection.QueryAsync<Receipt>(sql);
            return receipts;
        }

		public async Task<int> InsertReceiptAsync(ReceiptAddViewModel receipt)
		{
			await using var connection = new NpgsqlConnection(ConnectionString);
			await connection.OpenAsync();

			var id = await connection.ExecuteScalarAsync<int>(
				"INSERT INTO receipt (number, date) VALUES (@Number, @Date) RETURNING id",
				new { receipt.Number, receipt.Date }
			);

			return id;
		}

		public async Task InsertReceiptResourcesAsync(IEnumerable<ReceiptResource> resources)
		{
			await using var connection = new NpgsqlConnection(ConnectionString);
			await connection.OpenAsync();

			var sql = @"INSERT INTO receipt_resource (receipt_id, resource_id, unit_id, count) 
                         VALUES (@ReceiptId, @ResourceId, @UnitId, @Count)";

			await connection.ExecuteAsync(sql, resources);
		}

        public async Task<Receipt> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT id, number, date FROM receipt WHERE id = @id";
            return await connection.QueryFirstOrDefaultAsync<Receipt>(sql, new { id });
        }

        public async Task<IEnumerable<ReceiptResource>> GetResourcesByReceiptIdAsync(int receiptId)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = @"SELECT id, receipt_id AS ReceiptId, resource_id AS ResourceId, unit_id AS UnitId, count AS Count FROM receipt_resource WHERE receipt_id = @receiptId";

            return await connection.QueryAsync<ReceiptResource>(sql, new { receiptId });
        }

        public async Task<IEnumerable<int>> GetReceiptNumbersAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<int>("SELECT DISTINCT number FROM receipt ORDER BY number");
        }

        public async Task<IEnumerable<string>> GetResourcesAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<string>("SELECT DISTINCT id, name FROM resource where status = @Status ORDER BY name" , new { Status = 1 });
        }

        public async Task<IEnumerable<string>> GetUnitsAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<string>("SELECT DISTINCT id, name FROM unit where status = @Status ORDER BY name" , new { Status = 1 });
        }
        public async Task<DateTime?> GetEarliestDateAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<DateTime?>(
                "SELECT MIN(date) FROM receipt"
            );
        }

        public async Task<DateTime?> GetLatestDateAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryFirstOrDefaultAsync<DateTime?>(
                "SELECT MAX(date) FROM receipt"
            );
        }
        public async Task<IEnumerable<ResourceTable>> GetAllResourcesAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<ResourceTable>("SELECT id, name FROM resource where status = @Status", new { Status = 1 });
        }

        public async Task<IEnumerable<UnitTable>> GetAllUnitsAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            return await connection.QueryAsync<UnitTable>("SELECT id, name FROM unit where status = @Status", new { Status = 1 });
        }
        public async Task UpdateReceiptAsync(int id, int number, DateTime date, List<ReceiptResource> resources)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var updateReceiptSql = @"UPDATE receipt SET number = @Number, date = @Date WHERE id = @Id";
                await connection.ExecuteAsync(updateReceiptSql, new { Id = id, Number = number, Date = date }, transaction);

                var deleteResourcesSql = "DELETE FROM receipt_resource WHERE receipt_id = @ReceiptId";
                await connection.ExecuteAsync(deleteResourcesSql, new { ReceiptId = id }, transaction);

                var insertResourceSql = @"INSERT INTO receipt_resource (receipt_id, resource_id, unit_id, count) VALUES (@ReceiptId, @ResourceId, @UnitId, @Count)";

                foreach (var res in resources)
                {
                    await connection.ExecuteAsync(insertResourceSql, new
                    {
                        ReceiptId = id,
                        ResourceId = res.ResourceId,
                        UnitId = res.UnitId,
                        Count = res.Count
                    }, transaction);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteReceiptAsync(int id)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await connection.ExecuteAsync("DELETE FROM receipt_resource WHERE receipt_id = @Id", new { Id = id }, transaction);

                await connection.ExecuteAsync("DELETE FROM receipt WHERE id = @Id",new { Id = id }, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
