using Auto.WebAPI.Models;

interface IBranchesManager
{
    Task<List<Branch>> GetAllAsync();
}
