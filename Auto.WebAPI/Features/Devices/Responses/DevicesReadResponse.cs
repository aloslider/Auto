using Auto.WebAPI.Features.Devices.Dtos;

namespace Auto.WebAPI.Features.Devices.Responses;

class DevicesReadResponse
{
    public List<DeviceReadDto> Devices { get; set; }
}
