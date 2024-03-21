using Auto.WebAPI.Models;

namespace Auto.WebAPI.Database.Repositories.Devices;

interface IDevicesRepository
{
    Task<List<Device>> GetAllAsync();
    Task<List<Device>> GetAllByTypeAsync(string connectionType);
    Task<Device?> GetByIdAsync(int deviceId);
}
