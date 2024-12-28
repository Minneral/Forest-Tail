using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectApplesQuestStep : QuestStep
{
    private int collectedApples = 0;
    private int applesToComplete = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onAppleCollected += AppleCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onAppleCollected -= AppleCollected;
    }


    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    private void AppleCollected()
    {
        if(collectedApples < applesToComplete)
        {
                collectedApples++;
        }

        if(collectedApples >= applesToComplete)
        {
            FinishQuestStep();
        }
    }
}
