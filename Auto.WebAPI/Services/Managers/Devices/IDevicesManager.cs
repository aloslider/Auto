using Auto.WebAPI.Models;

interface IDevicesManager
{
    Task<Either<string, List<Device>>> GetAllAsync(string? connectionType);
}
