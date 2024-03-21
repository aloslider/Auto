namespace Auto.WebAPI.Models;

class Device
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ConnectionType ConnectionType { get; set; }
}