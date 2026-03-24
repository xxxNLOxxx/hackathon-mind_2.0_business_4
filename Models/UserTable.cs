using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class UserTable
{
    public int IdUser { get; set; }

    public string? Email { get; set; }

    public string? Pwd { get; set; }

    public int? IdRole { get; set; }

    public string? City { get; set; }

    public int? Age { get; set; }

    public string? FullName { get; set; }

    public DateOnly? Birthday { get; set; }

    public int? TotalPoints { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<EventTable> EventTables { get; set; } = new List<EventTable>();

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<OrganizerReview> OrganizerReviewIdAuthorNavigations { get; set; } = new List<OrganizerReview>();

    public virtual ICollection<OrganizerReview> OrganizerReviewIdOrganizerNavigations { get; set; } = new List<OrganizerReview>();

    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();

    public virtual ICollection<PointsHistory> PointsHistories { get; set; } = new List<PointsHistory>();

    // для системы призов
    public ICollection<Event> OrganizedEvents { get; set; }
}
