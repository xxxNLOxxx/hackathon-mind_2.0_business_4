using System;
using System.Collections.Generic;

namespace Hackaton.Models;

public partial class Reward
{
    public int IdReward { get; set; }

    public string RewardName { get; set; } = null!;

    public virtual ICollection<EventReward> EventRewards { get; set; } = new List<EventReward>();
}
