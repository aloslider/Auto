using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Devices.Dtos;
using Auto.WebAPI.Features.Devices.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Devices.Endpoints;

class GetAllDevicesEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var devicesGroup = routeBuilder.MapGroup("devices");
        devicesGroup.MapGet("all", async Task<Ok<DevicesReadResponse>>
            (string? connectionType, CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var devices = db.Devices.Include(d => d.ConnectionTypeNavigation)
                                    .Select(d => mapper.Map<DeviceReadDto>(d));
            return
                TypedResults.Ok(
                    mapper.Map<DevicesReadResponse>(
                        connectionType is not null
                        ? await devices.Where(d => d.ConnectionType == connectionType)
                                       .ToListAsync(ct)
                        : await devices.ToListAsync(ct)));
        })
        .WithTags("Devices")
        .WithSummary("Get all devices")
        .WithOpenApi(opt =>
        {
            opt.Parameters[0].Description = "Connection type";
            return opt;
        });
    }
}
