using Auto.WebAPI.Features.Devices.Dtos;

namespace Auto.WebAPI.Features.Installations.Dtos;

class InstallationReadDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Branch { get; set; }

    public int OrderNumber { get; set; }

    public bool IsDefault { get; set; }

    public DeviceReadDto Device { get; set; }
}
