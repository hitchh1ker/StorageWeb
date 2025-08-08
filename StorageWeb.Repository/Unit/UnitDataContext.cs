using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace StorageWeb.Repository.Unit.Models
{
    public class UnitDataContext
    {
        private readonly string ConnectionString;

        public UnitDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
        public async Task<IEnumerable<Unit>> GetAsync(int status)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT id, name as Name, status as Status FROM unit where status = @Id";
            var units = await connection.QueryAsync<Unit>(sql, new { Id = status });
            return units;
        }
        public async Task<Unit?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var sql = "SELECT id, name as Name, status as Status FROM unit WHERE id = @Id";
            var unit = await connection.QueryFirstOrDefaultAsync<Unit>(sql, new { Id = id });

            return unit;
        }
        public async Task InsertAsync(Unit unit)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("INSERT INTO unit (name, status) VALUES (@name, @status)",new { name = unit.Name, status = 1 }));
        }

        public async Task DeleteAsync(Unit unit)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("DELETE from unit WHERE id = @id", new { id = unit.Id }));
        }

        public async Task UpdateAsync(Unit unit)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE unit SET name = @name WHERE id = @id", new { id = unit.Id, name = unit.Name }));
        }
        public async Task UpdateStatusAsync(Unit unit)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE unit SET status = @status WHERE id = @id", new { id = unit.Id, status = 2 }));
        }
    }
}
