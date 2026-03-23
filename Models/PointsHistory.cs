using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class PointsHistory
{
    public int IdPointHistory { get; set; }

    public int? IdUser { get; set; }

    public double? PointsChange { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserTable? IdUserNavigation { get; set; }
}
