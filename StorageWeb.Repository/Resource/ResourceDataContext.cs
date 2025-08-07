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
        public async Task<IEnumerable<Resource>> GetAsync(int id)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var units = await connection.QueryAsync<Resource>($"SELECT id, name as Name, status as Status FROM resource where status = {id}");
            return units;
        }
        public async Task InsertAsync(Resource resource)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("INSERT INTO resource (name, status) VALUES (@name, @status)", new { name = resource.Name, status = resource.Status }));
        }

        public async Task DeleteAsync(Resource resource)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("DELETE from resource WHERE id = @id", new { id = resource.Id }));
        }

        public async Task UpdateAsync(Resource resource)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE resource SET name = @name WHERE id = @id", new { id = resource.Id, name = resource.Name }));
        }
        public async Task UpdateStatusAsync(Resource resource)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            await connection.ExecuteAsync(new CommandDefinition("UPDATE resource SET status = @status WHERE id = @id", new { id = resource.Id, status = 2 }));
        }
    }
}
