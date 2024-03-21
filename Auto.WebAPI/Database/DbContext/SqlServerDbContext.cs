using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Auto.WebAPI.Database.DbContext;

class SqlServerDbContext(IOptions<DbConfig> options) : IDbContext
{
    readonly IOptions<DbConfig> _options = options;

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_options.Value.ConnectionString);
    }
}
