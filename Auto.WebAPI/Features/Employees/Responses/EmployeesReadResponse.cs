using Auto.WebAPI.Features.Employees.Dtos;

namespace Auto.WebAPI.Features.Employees.Responses;

class EmployeesReadResponse
{
    public List<EmployeeReadDto> Employees { get; set; }
}
