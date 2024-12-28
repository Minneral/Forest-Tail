using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoblinsQuestReward : QuestReward
{
    public override void ClaimRewards()
    {
        this.stats.Heal(25);
    }
}
