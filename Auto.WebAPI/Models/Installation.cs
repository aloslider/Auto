namespace Auto.WebAPI.Models;

class Installation
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Branch Branch { get; set; }

    public Device Device { get; set; }

    public byte OrderNumber { get; set; }

    public bool IsDefault { get; set; }
}