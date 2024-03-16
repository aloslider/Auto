using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Installations.Dtos;
using Auto.WebAPI.Features.Installations.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Installations.Endpoints;

class GetInstallationByIdEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var installationsGroup = routeBuilder.MapGroup("installations");
        installationsGroup.MapGet("{id}", Results<Ok<InstallationReadResponse>, NotFound> 
        (int id, CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
        {
            var foundInstallation = db.Installations.Where(i => i.Id == id)
                                                    .Include(i => i.DeviceNavigation)
                                                    .Select(i => mapper.Map<InstallationReadDto>(i))
                                                    .FirstOrDefault();
            return
                foundInstallation is not null
              ? TypedResults.Ok(new InstallationReadResponse() { Installation = foundInstallation })
              : TypedResults.NotFound();
        })
        .WithTags("Installations")
        .WithSummary("Get installation by id")
        .WithDescription("Get installation by id.")
        .WithOrder(2)
        .WithOpenApi(opt =>
        {
            opt.Parameters[0].Description = "Installation id";
            return opt;
        });
    }
}
