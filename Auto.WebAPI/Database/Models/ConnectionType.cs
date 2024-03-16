using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class ConnectionType
{
    public string Type { get; set; } = null!;

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
