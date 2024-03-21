using Auto.WebAPI.Models;

namespace Auto.WebAPI.Database.Repositories.Installations;

interface IInstallationRepository
{
    Task<List<Installation>> GetAllAsync();
    Task<Installation?> GetByIdAsync(int id);
    Task<List<Installation>> GetBranchInstallationsAsync(int branchId);
    int Create(Installation newInstallation);
    Task DeleteAsync(int id);
    Task UpdateAsync(int id, Installation installation);
}
