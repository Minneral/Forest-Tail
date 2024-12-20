using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectMushroomsReward : QuestReward
{
    public override void ClaimRewards()
    {
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();
        Item mushroom = ScriptableObject.CreateInstance<Item>();
        mushroom.name = "Гриб";
        mushroom.type = ItemType.Food;

        for (int i = 0; i < 6; i++)
            inventory.RemoveItem(mushroom);
    }
}
