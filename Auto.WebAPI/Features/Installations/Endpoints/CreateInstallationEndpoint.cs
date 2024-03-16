
using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Installations.Requests;
using Auto.WebAPI.Features.Installations.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auto.WebAPI.Features.Installations.Endpoints;

class CreateInstallationEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var installationsGroup = routeBuilder.MapGroup("installations");
        installationsGroup.MapPost("create", async Task<Results<CreatedAtRoute<InstallationCreateResponse>, BadRequest<ErrorResponse>>> 
            (InstallationCreateRequest req, 
             CompanyDbContext db, 
             IMapper mapper, 
             IOptions<InstallationsOptions> options, 
             CancellationToken ct) =>
        {
            // Check branch exists
            Branch? foundBranch = await db.Branches.Where(b => b.Name == req.Branch).FirstOrDefaultAsync(ct);

            if (foundBranch is null)
            {
                return TypedResults.BadRequest<ErrorResponse>("Branch does not exist.");
            }

            // Check device exists
            Device? foundDevice = await db.Devices.Where(d => d.Name == req.DeviceName).FirstOrDefaultAsync(ct);

            if (foundDevice is null)
            {
                return TypedResults.BadRequest<ErrorResponse>("Device does not exist.");
            }

            // Check installations count
            var branchInstallations = await db.Installations.Where(i => i.Branch == req.Branch).ToListAsync(ct);
            int branchInstallationsCount = branchInstallations.Count;

            if (branchInstallationsCount >= options.Value.MaxCount)
            {
                return TypedResults.BadRequest<ErrorResponse>("Installations count exceeded max number constraint.");
            }

            if (req.OrderNumber.HasValue)
            {
                bool sameOrderNumExists = branchInstallations.Where(i => i.OrderNumber == req.OrderNumber).Any();

                if (sameOrderNumExists)
                {
                    return TypedResults.BadRequest<ErrorResponse>("Installation with the same order number already exists.");
                }
            }
            else
            {
                req.OrderNumber = branchInstallationsCount + 1;
            }

            // Check default installation
            bool defaultInstallationExists = branchInstallations.FirstOrDefault(i => i.IsDefault == true) is not null;

            if (req.IsDefault && defaultInstallationExists)
            {
                return TypedResults.BadRequest<ErrorResponse>("Default installation already exists.");
            }

            if (!req.IsDefault && !defaultInstallationExists)
            {
                return TypedResults.BadRequest<ErrorResponse>("At least one installation in the branch must be set as default.");
            }

            var addedInstallation = db.Installations.Add(new()
            {
                Name = req.Name,
                OrderNumber = req.OrderNumber.Value,
                BranchNavigation = foundBranch,
                DeviceNavigation = foundDevice
            });

            await db.SaveChangesAsync(ct);
            return TypedResults.CreatedAtRoute(mapper.Map<InstallationCreateResponse>(addedInstallation));
        })
        .WithTags("Installations")
        .WithSummary("Create new installation")
        .WithOrder(3)
        .WithDescription(
            "Create new installation. " +
            "Order number should be uniqe. " +
            "At least one installation in the branch should be set as default.")
        .WithOpenApi();
    }
}