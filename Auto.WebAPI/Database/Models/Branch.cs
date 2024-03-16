using System;
using System.Collections.Generic;

namespace Auto.WebAPI.Database.Models;

public partial class Branch
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Installation> Installations { get; set; } = new List<Installation>();
}
