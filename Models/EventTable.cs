using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class EventTable
{
    public int IdEvent { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public int? IdOrganizer { get; set; }

    public int? IdCategory { get; set; }

    public int? BasePoints { get; set; }

    public double? ComplexityCoeff { get; set; }

    public int? IdStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<EventReward> EventRewards { get; set; } = new List<EventReward>();

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual UserTable? IdOrganizerNavigation { get; set; }

    public virtual StatusEvent? IdStatusNavigation { get; set; }

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
}
