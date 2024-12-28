using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CollectMushroomStep : QuestStep
{
    string itemIdToCollect = "Mushroom";
    int collected = 0;
    int needToCollect = 6;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onMushroomCollected += Perform;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onMushroomCollected -= Perform;
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                collected = inventory.GetSlotsByItemId(itemIdToCollect).Sum(i => i.amount);
                if (collected >= needToCollect)
                    FinishQuestStep();
            }
        }
    }

    void Perform()
    {
        if (collected < needToCollect)
            collected++;

        if (collected >= needToCollect)
            FinishQuestStep();
    }
    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
