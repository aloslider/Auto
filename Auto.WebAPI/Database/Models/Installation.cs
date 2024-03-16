using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class Installation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public string Device { get; set; } = null!;

    public int OrderNumber { get; set; }

    public bool IsDefault { get; set; }

    public virtual Branch BranchNavigation { get; set; } = null!;

    public virtual Device DeviceNavigation { get; set; } = null!;
}
