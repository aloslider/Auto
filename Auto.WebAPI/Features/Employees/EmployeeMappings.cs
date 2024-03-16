using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Employees.Dtos;
using Auto.WebAPI.Features.Employees.Responses;
using AutoMapper;

class EmployeeMappings : Profile
{
    public EmployeeMappings()
    {
        CreateMap<Employee, EmployeeReadDto>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.Name));
        CreateMap<List<EmployeeReadDto>, EmployeesReadResponse>()
            .ForMember(d => d.Employees, o => o.MapFrom(src => src));
    }
}
