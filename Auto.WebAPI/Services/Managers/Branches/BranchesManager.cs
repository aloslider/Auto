using Auto.WebAPI.Database.Repositories.Branches;
using Auto.WebAPI.Models;

class BranchesManager(IBranchesRepository db) : IBranchesManager
{
    readonly IBranchesRepository _db = db;

    public async Task<List<Branch>> GetAllAsync()
    {
        return await _db.GetAllAsync();
    }
}
