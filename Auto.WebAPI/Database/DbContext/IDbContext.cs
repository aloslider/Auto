using System.Data;

namespace Auto.WebAPI.Database.DbContext;

interface IDbContext
{
    IDbConnection CreateConnection();
}
