using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.PrintTasks.Requests;
using Auto.WebAPI.Features.PrintTasks.Responses;
using Auto.WebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.PrintTasks.Endpoints;

class PrintTaskCreateEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var printTasksGroup = routeBuilder.MapGroup("print");
        printTasksGroup.MapPost("create", 
            async Task<Results<Ok<PrintTaskCreateResponse>, BadRequest<ErrorResponse>>>
                (PrintTaskCreateRequest req,
                 CompanyDbContext db,
                 IPrintService printService,
                 IMapper mapper,
                 CancellationToken ct) =>
            {
                var installations = await db.Employees.Where(e => e.Id == req.EmployeeId)
                                                      .SelectMany(e => db.Branches.Where(b => e.Branch == b.Name))
                                                      .Join(db.Installations,
                                                            branch => branch.Name,
                                                            installation => installation.Branch,
                                                            (_, installation) => installation)
                                                      .ToListAsync(ct);

                if (installations.Count == 0)
                {
                    return TypedResults.BadRequest<ErrorResponse>("No installations found.");
                }

                var foundDevice =
                    req.DeviceOrderNumber.HasValue
                    ? installations.Where(i => i.OrderNumber == req.DeviceOrderNumber).FirstOrDefault()
                    : installations.Find(i => i.IsDefault == true);

                if (foundDevice is null)
                {
                    return TypedResults.BadRequest<ErrorResponse>("No installation found with provided order number.");
                }

                bool result = await printService.Print(ct);
                var printTask = db.PrintTasks.Add(new()
                {
                    Name = req.Name,
                    EmployeeId = req.EmployeeId,
                    DeviceOrderNum = foundDevice.OrderNumber,
                    PageCount = req.PageCount,
                    Status = result
                });
                await db.SaveChangesAsync(ct);
                return TypedResults.Ok(mapper.Map<PrintTaskCreateResponse>(result ? "Успех" : "Неудача"));
            })
        .WithTags("Print")
        .WithSummary("Create print task")
        .WithDescription("Create print task. After the delay of 1-4s returns task status.")
        .WithOpenApi();
    }
}
