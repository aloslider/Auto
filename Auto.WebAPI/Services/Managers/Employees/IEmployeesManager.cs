using Auto.WebAPI.Models;

interface IEmployeesManager
{
    Task<List<Employee>> GetAllAsync();
}
