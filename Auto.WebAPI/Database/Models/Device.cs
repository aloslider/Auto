using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class Device
{
    public string Name { get; set; } = null!;

    public string ConnectionType { get; set; } = null!;

    public virtual ConnectionType ConnectionTypeNavigation { get; set; } = null!;

    public virtual ICollection<Installation> Installations { get; set; } = new List<Installation>();
}
