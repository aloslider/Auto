using Auto.WebAPI.Features.PrintTasks.Responses;
using AutoMapper;

namespace Auto.WebAPI.Features.PrintTasks;

class PrintTaskMappings : Profile
{
	public PrintTaskMappings()
	{
		CreateMap<string, PrintTaskCreateResponse>()
			.ForMember(res => res.Result, o => o.MapFrom(s => s));
    }
}
