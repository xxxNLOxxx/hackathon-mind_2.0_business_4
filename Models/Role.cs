using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class Role
{
    public int IdRole { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<UserTable> UserTables { get; set; } = new List<UserTable>();
}
