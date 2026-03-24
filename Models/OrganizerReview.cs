using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class OrganizerReview
{
    public int IdOrganizerReview { get; set; }

    public int? IdOrganizer { get; set; }

    public int? IdAuthor { get; set; }

    public int? Rating { get; set; }

    public string? CommentReview { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserTable? IdAuthorNavigation { get; set; }

    public virtual UserTable? IdOrganizerNavigation { get; set; }
}
