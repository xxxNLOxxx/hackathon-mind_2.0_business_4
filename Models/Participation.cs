using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class Participation
{
    public int IdParticipation { get; set; }

    public int? IdUser { get; set; }

    public int? IdEvent { get; set; }

    public int? IdStatusParticipation { get; set; }

    public string? QrCodeHash { get; set; }

    public double? PointsEarned { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public virtual EventTable? IdEventNavigation { get; set; }

    public virtual StatusParticipation? IdStatusParticipationNavigation { get; set; }

    public virtual UserTable? IdUserNavigation { get; set; }
}
