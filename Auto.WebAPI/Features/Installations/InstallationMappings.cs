using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Installations.Dtos;
using Auto.WebAPI.Features.Installations.Responses;
using AutoMapper;

class InstallationMappings : Profile
{
    public InstallationMappings()
    {
        CreateMap<Installation, InstallationReadDto>()
            .ForMember(d => d.Device, o => o.MapFrom(src => src.DeviceNavigation));
        CreateMap<List<InstallationReadDto>, InstallationsReadResponse>()
            .ForMember(d => d.Installations, o => o.MapFrom(src => src));
    }
}

class InstallationReadDtoToResponseConverter : ITypeConverter<InstallationReadDto, InstallationsReadResponse>
{
    public InstallationsReadResponse Convert(InstallationReadDto source, InstallationsReadResponse destination, ResolutionContext context)
    {
        destination = new();
        destination.Installations = [];
        destination.Installations.Add(source);
        return destination;
    }
}
