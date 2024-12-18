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
}