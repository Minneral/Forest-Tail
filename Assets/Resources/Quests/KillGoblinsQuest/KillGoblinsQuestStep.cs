using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillGoblinsQuestStep : QuestStep
{
    private int killed = 0;
    private int killsToComplete = 2;

    private void OnEnable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath += KilledComplete;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath -= KilledComplete;
    }

    void Start()
    {
        killed = GameEventsManager.instance.npcEvents.enemiesKilld.GoblinsKilled;
        if(killed >= killsToComplete)
            FinishQuestStep();
    }

    private void KilledComplete(NPCTypes type)
    {
        if (type != NPCTypes.Goblin)
            return;

        if (killed < killsToComplete)
        {
            killed++;
        }

        if (killed >= killsToComplete)
        {
            FinishQuestStep();
        }
    }

    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
