using Auto.WebAPI.Database.DbContext;
using Auto.WebAPI.Models;
using Dapper;

namespace Auto.WebAPI.Database.Repositories.Installations;

class InstallationRepository(IDbContext db) : IInstallationRepository
{
    readonly IDbContext _db = db;

    public int Create(Installation newInstallation)
    {
        using var conn = _db.CreateConnection();
        int id = conn.QuerySingle<int>(
            $"""
            INSERT INTO Installations (Name, BranchId, DeviceId, OrderNumber, IsDefault)
            OUTPUT INSERTED.Id
            VALUES (@name, @branchId, @deviceId, @orderNumber, @isDefault);
            """,
            param: new
            {
                name = newInstallation.Name,
                branchId = newInstallation.Branch.Id,
                deviceId = newInstallation.Device.Id,
                orderNumber = newInstallation.OrderNumber,
                isDefault = newInstallation.IsDefault
            });
        return id;
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = _db.CreateConnection();
        var rowsAff = await conn.ExecuteAsync(
            $"""
            DELETE FROM Installations
            WHERE Id = @id
            """,
            param: new { id });
        return;
    }

    public async Task<List<Installation>> GetAllAsync()
    {
        using var conn = _db.CreateConnection();
        var installations = await conn.QueryAsync(
            $"""
            SELECT *
            FROM Installations AS I
            JOIN Branches AS B ON I.BranchId = B.Id
            JOIN Devices AS D ON I.DeviceId = D.Id
            JOIN ConnectionTypes AS CT ON D.ConnectionTypeId = CT.Id
            """,
            map: (Models.Installation i, Branch b, Device d, ConnectionType ct) =>
            {
                d.ConnectionType = ct;
                i.Branch = b;
                i.Device = d;
                return i;
            });
        return installations.ToList();
    }

    public async Task<List<Installation>> GetBranchInstallationsAsync(int branchId)
    {
        using var conn = _db.CreateConnection();
        var installations = await conn.QueryAsync(
            $"""
            SELECT *
            FROM Installations AS I
            JOIN Branches AS B ON I.BranchId = B.Id
            JOIN Devices AS D ON I.DeviceId = D.Id
            JOIN ConnectionTypes AS CT ON D.ConnectionTypeId = CT.Id
            WHERE B.Id = @branchId
            """,
            param: new { branchId },
            map: (Installation i, Branch b, Device d, ConnectionType ct) =>
            {
                d.ConnectionType = ct;
                i.Branch = b;
                i.Device = d;
                return i;
            });
        return installations.ToList();
    }

    public async Task<Installation?> GetByIdAsync(int id)
    {
        using var conn = _db.CreateConnection();
        var installation = (await conn.QueryAsync(
            $"""
            SELECT *
            FROM Installations AS I
            JOIN Branches AS B ON I.BranchId = B.Id
            JOIN Devices AS D ON I.DeviceId = D.Id
            JOIN ConnectionTypes AS CT ON D.ConnectionTypeId = CT.Id
            WHERE I.Id = @id
            """, 
            map: (Installation i, Branch b, Device d, ConnectionType ct) =>
            {
                d.ConnectionType = ct;
                i.Branch = b;
                i.Device = d;
                return i;
            },
            param: new { id })).SingleOrDefault();
        return installation;
    }

    public async Task UpdateAsync(int id, Installation installation)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            $"""
            UPDATE Installations
            SET 
                Name = @name,
                BranchId = @branchId,
                DeviceId = @deviceId,
                OrderNumber = @orderNumber,
                IsDefault = @isDefault
            WHERE Id = @id
            """,
            param: new
            {
                id,
                name = installation.Name,
                branchId = installation.Branch.Id,
                deviceId = installation.Device.Id,
                orderNumber = installation.OrderNumber,
                isDefault = installation.IsDefault
            });
    }
}
