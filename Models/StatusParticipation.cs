using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class StatusParticipation
{
    public int IdStatusParticipation { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
}
