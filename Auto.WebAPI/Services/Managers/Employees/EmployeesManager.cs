using Auto.WebAPI.Database.Repositories.Employees;
using Auto.WebAPI.Models;

class EmployeesManager(IEmployeesRepository db) : IEmployeesManager
{
    readonly IEmployeesRepository _db = db;

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _db.GetAllAsync();
    }
}
