using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Branches.Dtos;
using Auto.WebAPI.Features.Branches.Responses;
using AutoMapper;

class BranchMappings : Profile
{
    public BranchMappings()
    {
        CreateMap<Branch, BranchReadDto>();
            //.ForMember(d => d.Name, o => o.MapFrom(src => src.Name));
        CreateMap<List<BranchReadDto>, BranchesReadResponse>()
            .ForMember(d => d.Branches, o => o.MapFrom(src => src));
    }
}
