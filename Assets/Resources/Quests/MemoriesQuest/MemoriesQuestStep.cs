using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoriesQuestStep : QuestStep
{
    private void OnEnable()
    {
        GameEventsManager.instance.puzzleEvents.onMemoriesPlayerWon += Perform;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.puzzleEvents.onMemoriesPlayerWon -= Perform;
    }

    void Perform()
    {
        FinishQuestStep();
    }

    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
