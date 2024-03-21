using Auto.WebAPI.Database.Repositories.Devices;
using Auto.WebAPI.Models;
using static Prelude;

class DevicessManager(IDevicesRepository db) : IDevicesManager
{
    readonly IDevicesRepository _db = db;

    public async Task<Either<string, List<Device>>> GetAllAsync(string? connectionType)
    {
        if (connectionType is not null)
        {
            return Right(await _db.GetAllByTypeAsync(connectionType));
        }

        return Right(await _db.GetAllAsync());
    }
}
