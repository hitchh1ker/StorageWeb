using Microsoft.Extensions.Options;
using Npgsql;

namespace StorageWeb.Repository.Unit
{
    public class UnitDataContext
    {
        private readonly string ConnectionString;

        public UnitDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
        public async Task GetAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }
        public async Task InsertAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }

        public async Task DeleteAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }

        public async Task UpdateAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }
        public async Task UpdateStatusAsync()
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
        }
    }
}
