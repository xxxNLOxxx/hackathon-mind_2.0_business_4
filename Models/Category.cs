using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<EventTable> EventTables { get; set; } = new List<EventTable>();
}
