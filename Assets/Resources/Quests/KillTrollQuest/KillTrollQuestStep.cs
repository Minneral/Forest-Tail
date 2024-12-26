using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrollQuestStep : QuestStep
{
    protected override void SetQuestStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath += KilledComplete;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath -= KilledComplete;
    }

    private void KilledComplete(NPCTypes type)
    {
        if (type == NPCTypes.Troll)
            FinishQuestStep();

    }
}
