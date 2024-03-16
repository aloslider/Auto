using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Devices.Dtos;
using Auto.WebAPI.Features.Devices.Responses;
using AutoMapper;

class DeviceMappings : Profile
{
    public DeviceMappings()
    {
        CreateMap<Device, DeviceReadDto>()
            .ForMember(d => d.Name, o => o.MapFrom(src => src.Name));
        CreateMap<List<DeviceReadDto>, DevicesReadResponse>()
            .ForMember(d => d.Devices, o => o.MapFrom(src => src));
    }
}
