using Auto.WebAPI.Features.Installations.Dtos;

namespace Auto.WebAPI.Features.Installations.Responses;

class InstallationsReadResponse
{
    public List<InstallationReadDto> Installations { get; set; }
}
