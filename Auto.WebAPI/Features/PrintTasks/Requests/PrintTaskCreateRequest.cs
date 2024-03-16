namespace Auto.WebAPI.Features.PrintTasks.Requests;

class PrintTaskCreateRequest
{
    public string? Name { get; set; }

    public int EmployeeId { get; set; }

    public int? DeviceOrderNumber { get; set; }

    public int PageCount { get; set; }
}
