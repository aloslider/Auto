using Auto.WebAPI.Dtos;
using Auto.WebAPI.Models;

interface IInstallationsManager
{
    Task<Either<string, Installation>> GetByIdAsync(int id);
    Task<List<Installation>> GetAllAsync();
    Task<Either<string, int>> CreateAsync(InstallationDto installationDto);
    Task DeleteAsync(int id);
}
