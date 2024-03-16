namespace Auto.WebAPI.Features.Installations.Requests;

class InstallationCreateRequest
{
    public string? Name { get; set; }

    public string? Branch { get; set; }

    public string? DeviceName { get; set; }

    public int? OrderNumber { get; set; }

    public bool IsDefault { get; set; }
}