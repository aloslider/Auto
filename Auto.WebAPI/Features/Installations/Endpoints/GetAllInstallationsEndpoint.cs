using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Installations.Dtos;
using Auto.WebAPI.Features.Installations.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Installations.Endpoints;

class GetAllInstallationsEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var installationsGroup = routeBuilder.MapGroup("installations");
        installationsGroup.MapGet("all",
            async Task<Ok<InstallationsReadResponse>> (CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
                TypedResults.Ok(
                    mapper.Map<InstallationsReadResponse>(
                        await db.Installations.Include(i => i.DeviceNavigation)
                                              .Select(i => mapper.Map<InstallationReadDto>(i))
                                              .ToListAsync(ct))))
        .WithTags("Installations")
        .WithSummary("Get all installations")
        .WithDescription("Get all installations.")
        .WithOrder(1)
        .WithOpenApi();
    }
}
