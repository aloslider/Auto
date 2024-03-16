using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.PrintTasks.Models;
using Auto.WebAPI.Features.PrintTasks.Responses;
using Auto.WebAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Auto.WebAPI.Features.PrintTasks.Endpoints;

class PrintTaskCreateCsvEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var printTasksGroup = routeBuilder.MapGroup("print");
        printTasksGroup.MapPost("create-csv", 
            async Task<Results<Ok<PrintTaskCsvCreateResponse>, UnprocessableEntity, BadRequest<ErrorResponse>>>
                (IFormFile file,
                 CompanyDbContext db,
                 ISessionLineParser parser,
                 IMapper mapper,
                 IOptions<CsvFileParsingOptions> options,
                 CancellationToken ct) =>
            {
                if (file is null)
                {
                    return TypedResults.UnprocessableEntity();
                }

                if (file.ContentType != "text/csv")
                {
                    return TypedResults.UnprocessableEntity();
                }

                using var tr = new StreamReader(file.OpenReadStream());
                string? line;
                int rowNum = 0;
                List<Session> parsedSessions = [];

                while ((line = await tr.ReadLineAsync(ct)) is not null && rowNum < options.Value.RowsMaxCount)
                {
                    var session = parser.Parse(line);

                    if (session.IsFailed)
                    {
                        return TypedResults.BadRequest<ErrorResponse>("Parsing error.");
                    }

                    parsedSessions.Add(session.Value);
                    rowNum++;
                }

                await db.PrintTasks.AddRangeAsync(
                    parsedSessions.Select(s => new PrintTask()
                    {
                        Name = s.Name,
                        EmployeeId = s.EmployeeId,
                        DeviceOrderNum = s.DeviceOrderNumber,
                        PageCount = s.PageCount
                    }), ct);
                int sessionsCreated = await db.SaveChangesAsync(ct);
                return TypedResults.Ok(new PrintTaskCsvCreateResponse() { SessionsCreated = sessionsCreated });
            })
        .WithTags("Print")
        .WithSummary("Create print tasks")
        .WithDescription("Create print tasks from the CSV file. Returns number of tasks executed.")
        .WithOpenApi()
        .DisableAntiforgery();
    }
}
