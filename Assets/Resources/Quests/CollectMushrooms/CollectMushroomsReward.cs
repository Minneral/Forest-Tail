using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMushroomsReward : QuestReward
{
    public override void ClaimRewards()
    {
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();

        for (int i = 0; i < 6; i++)
            inventory.RemoveItem("Mushroom");
    }
}
