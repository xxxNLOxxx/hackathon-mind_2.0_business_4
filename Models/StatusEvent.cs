using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class StatusEvent
{
    public int IdStatus { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<EventTable> EventTables { get; set; } = new List<EventTable>();
}
