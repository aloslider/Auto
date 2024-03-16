using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public virtual Branch BranchNavigation { get; set; } = null!;

    public virtual ICollection<PrintTask> PrintTasks { get; set; } = new List<PrintTask>();
}
