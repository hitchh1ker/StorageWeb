using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace StorageWeb.Repository.Resource.Models
{
    public class ResourceDataContext
    {
        private readonly string ConnectionString;

        public ResourceDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
        public async Task<IEnumerable<Resource>> GetAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var units = await connection.QueryAsync<Resource>("SELECT id, name as Name, status as Status FROM resource where status = 1");
            return units;
        }
        public async Task InsertAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("INSERT INTO resource (name, status) VALUES (@name, @status)", new { }));
        }

        public async Task DeleteAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("DELETE from resource WHERE id = @id", new { }));
        }

        public async Task UpdateAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE from resource WHERE id = @id", new { }));
        }
        public async Task UpdateStatusAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE from resource WHERE id = @id", new { }));
        }
    }
}
