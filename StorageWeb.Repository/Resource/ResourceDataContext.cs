using Microsoft.Extensions.Options;

namespace StorageWeb.Repository.Resource
{
    public class ResourceDataContext
    {
        private readonly string ConnectionString;

        public ResourceDataContext(IOptions<ConnectionString> options)
        {
            ConnectionString = options.Value.StorageDb;
        }
    }
}
