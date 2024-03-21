namespace Auto.WebAPI.Dtos;

class InstallationDto
{
    public string? Name { get; set; }

    public int BranchId { get; set; }

    public int DeviceId { get; set; }

    public byte? OrderNumber { get; set; }

    public bool IsDefault { get; set; }
}
