using Auto.WebAPI.Database.DbContext;
using Auto.WebAPI.Models;
using Dapper;

namespace Auto.WebAPI.Database.Repositories.Employees;

class EmployeesRepository(IDbContext db) : IEmployeesRepository
{
    readonly IDbContext _db = db;

    public async Task<List<Employee>> GetAllAsync()
    {
        using var conn = _db.CreateConnection();
        string sql = "SELECT * " +
                     "FROM [Employees] AS E " +
                     "JOIN [Branches] AS B ON E.[BranchId] = B.[Id]";
        var employees = await conn.QueryAsync<Employee, Branch, Employee>(sql,
            (e, b) => { e.Branch = b; return e; });
        return employees.ToList();
    }
}
