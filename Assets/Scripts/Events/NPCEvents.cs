using System;

public class NPCEvents
{
    public event Action onNPCDeath;
    public void NPCDeath()
    {
        if (onNPCDeath != null)
        {
            onNPCDeath();
        }
    }

    public event Action<string> onNPCTalked;
    public void NPCTalked(string npcName)
    {
        if (onNPCTalked != null)
        {
            onNPCTalked(npcName);
        }
    }
}