using Auto.WebAPI.Models;

namespace Auto.WebAPI.Database.Repositories.Branches;

interface IBranchesRepository
{
    Task<Branch?> GetByIdAsync(int id);
    Task<List<Branch>> GetAllAsync();
}
