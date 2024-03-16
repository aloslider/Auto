using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Employees.Dtos;
using Auto.WebAPI.Features.Employees.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Employees.Endpoints;

class GetAllEmployeesEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var employeesGroup = routeBuilder.MapGroup("employees");
        employeesGroup.MapGet("all", async Task<Ok<EmployeesReadResponse>> (CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
            TypedResults.Ok(
                mapper.Map<EmployeesReadResponse>(
                    await db.Employees.Include(e => e.BranchNavigation)
                                      .Select(e => mapper.Map<EmployeeReadDto>(e))
                                      .ToListAsync(ct))))
        .WithTags("Employees")
        .WithSummary("Get all employees")
        .WithOpenApi();
    }
}
