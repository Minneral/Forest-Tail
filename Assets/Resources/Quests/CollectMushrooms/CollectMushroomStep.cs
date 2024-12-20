using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectMushroomStep : QuestStep
{
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
