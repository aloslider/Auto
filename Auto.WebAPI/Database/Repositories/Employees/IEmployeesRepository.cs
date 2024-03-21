using Auto.WebAPI.Models;

namespace Auto.WebAPI.Database.Repositories.Employees;

interface IEmployeesRepository
{
    Task<List<Employee>> GetAllAsync();
}
