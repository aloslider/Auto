using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Branches.Dtos;
using Auto.WebAPI.Features.Branches.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Features.Branches.Endpoints;

class GetAllBranchesEndpoint : IMinimalEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var branchesGroup = routeBuilder.MapGroup("branches");
        branchesGroup.MapGet("all",
            async Task<Ok<BranchesReadResponse>> (CompanyDbContext db, IMapper mapper, CancellationToken ct) =>
                TypedResults.Ok(
                    mapper.Map<BranchesReadResponse>(
                        await db.Branches.Select(b => mapper.Map<BranchReadDto>(b)).ToListAsync(ct))))
        .WithTags("Branches")
        .WithSummary("Get all branches")
        .WithOpenApi();
    }
}
