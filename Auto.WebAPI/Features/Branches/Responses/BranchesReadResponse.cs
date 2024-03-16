using Auto.WebAPI.Features.Branches.Dtos;

namespace Auto.WebAPI.Features.Branches.Responses;

class BranchesReadResponse
{
    public List<BranchReadDto> Branches { get; set; }
}
