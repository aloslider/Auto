
using Auto.WebAPI.Database.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Installations.Endpoints;

class DeleteInstallationEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var installationsGroup = routeBuilder.MapGroup("installations");
        installationsGroup.MapDelete("delete", async Task<Results<Ok, BadRequest<ErrorResponse>>> (int id, CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
        {            
            var installationToRemove = await db.Installations.Where(i => i.Id == id).FirstOrDefaultAsync(ct);

            if (installationToRemove is null)
            {
                return TypedResults.Ok();
            }

            var otherInstallations = await db.Installations.Where(i => i.Branch == installationToRemove.Branch)
                                                            .Where(i => i.Id != installationToRemove.Id)
                                                            .OrderBy(i => i.OrderNumber)                               
                                                            .ToListAsync(ct);
            var isSingleInstallationInTheBranch = otherInstallations.Count == 0;

            if (installationToRemove.IsDefault)
            {
                if (isSingleInstallationInTheBranch)
                {
                    return TypedResults.BadRequest<ErrorResponse>("At least one default installation should exist in the branch.");
                }

                // Set new default installtion
                var newDefaultInstallation = otherInstallations[0]!;
                newDefaultInstallation.IsDefault = true;
                db.Installations.Remove(installationToRemove);
                db.Installations.Update(newDefaultInstallation);
                db.SaveChanges();
                return TypedResults.Ok();
            }

            db.Installations.Remove(installationToRemove);
            db.SaveChanges();
            return TypedResults.Ok();
        })
        .WithTags("Installations")
        .WithSummary("Delete specific installation")
        .WithDescription(
            "Delete specific installation. " +
            "At least one default installation should exist in the branch.")
        .WithOrder(4)
        .WithOpenApi(opt =>
        {
            opt.Parameters[0].Description = "Installation id";
            return opt;
        });
    }
}