using Auto.WebAPI.Database.DbContext;
using Auto.WebAPI.Models;
using Dapper;

namespace Auto.WebAPI.Database.Repositories.Devices;

class DeviceRepository(IDbContext db) : IDevicesRepository
{
    readonly IDbContext _db = db;

    public async Task<Device?> GetByIdAsync(int id)
    {
        using var conn = _db.CreateConnection();
        var device = await conn.QuerySingleOrDefaultAsync<Device>(
            "SELECT * FROM Devices AS D WHERE D.Id = @id",
            param: new { id });
        return device;
    }

    public async Task<List<Device>> GetAllAsync()
    {
        using var conn = _db.CreateConnection();
        var devices = await conn.QueryAsync<Device, ConnectionType, Device>(
            $"""
            SELECT 
                D.[Id],
                D.[Name],
                CT.[Type]
            FROM Devices AS D
            JOIN ConnectionTypes AS CT ON D.ConnectionTypeId = CT.[Id]
            """,
            map: (d, ct) =>
            {
                d.ConnectionType = ct;
                return d;
            },
            splitOn: "Type");
        return devices.ToList();
    }

    public async Task<List<Device>> GetAllByTypeAsync(string connectionType)
    {
        using var conn = _db.CreateConnection();
        var devices = await conn.QueryAsync<Device, ConnectionType, Device>(
            $"""
            SELECT 
                D.[Id],
                D.[Name],
                CT.[Type]
            FROM Devices AS D
            JOIN ConnectionTypes AS CT ON D.ConnectionTypeId = CT.[Id]
            WHERE CT.[Type] = @conType
            """,
            map: (d, ct) =>
            {
                d.ConnectionType = ct;
                return d;
            },
            param: new { conType = connectionType },
            splitOn: "Type");
        return devices.ToList();
    }
}
