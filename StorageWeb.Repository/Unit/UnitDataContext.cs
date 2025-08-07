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
        public async Task<IEnumerable<Unit>> GetAsync(int id)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var units = await connection.QueryAsync<Unit>($"SELECT id, name as Name, status as Status FROM unit where status = {id}");
            return units;
        }
        public async Task InsertAsync(Unit unit)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("INSERT INTO unit (name, status) VALUES (@name, @status)",new { name = unit.Name, status = 1 }));
        }

        public async Task DeleteAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("DELETE from unit WHERE id = @id", new { }));
        }

        public async Task UpdateAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE from unit WHERE id = @id", new { }));
        }
        public async Task UpdateStatusAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE from unit WHERE id = @id", new { }));
        }
    }
}
