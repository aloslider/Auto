using Auto.WebAPI.Database.DbContext;
using Auto.WebAPI.Models;
using Dapper;

namespace Auto.WebAPI.Database.Repositories.Branches;

class BranchesRepository(IDbContext dbContext) : IBranchesRepository
{
    readonly IDbContext _db = dbContext;

    public async Task<List<Branch>> GetAllAsync()
    {
        using var conn = _db.CreateConnection();
        var branches = await conn.QueryAsync<Branch>("SELECT * FROM Branches");
        return branches.ToList();
    }

    public async Task<Branch?> GetByIdAsync(int id)
    {
        using var conn = _db.CreateConnection();
        var branch = await conn.QuerySingleOrDefaultAsync<Branch>(
            "SELECT * FROM Branches AS B WHERE B.Id = @id",
            param: new { id });
        return branch;
    }
}
