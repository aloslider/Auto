using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class PrintTask
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int EmployeeId { get; set; }

    public int DeviceOrderNum { get; set; }

    public int PageCount { get; set; }

    public bool Status { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
