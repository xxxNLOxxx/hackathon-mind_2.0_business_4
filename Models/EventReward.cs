using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class EventReward
{
    public int IdEventRewards { get; set; }

    public int? IdEvent { get; set; }

    public int? IdReward { get; set; }

    public string Title { get; set; } = null!;

    public int? Quantity { get; set; }

    public virtual EventTable? IdEventNavigation { get; set; }

    public virtual Reward? IdRewardNavigation { get; set; }
}
