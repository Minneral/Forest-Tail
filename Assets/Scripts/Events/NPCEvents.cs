using System;

public enum NPCTypes
{
    Goblin,
    Villager,
    Troll,
};

public class EnemiesKilld
{
    // Count kill of every NPC type for statistic
    public int GoblinsKilled = 0;
    public int TrollsKilled = 0;
}

public class NPCEvents
{
    public EnemiesKilld enemiesKilld = new EnemiesKilld();
    public event Action<NPCTypes> onNPCDeath;
    public void NPCDeath(NPCTypes type)
    {
        if (onNPCDeath != null)
        {
            onNPCDeath(type);

            switch (type)
            {
                case NPCTypes.Goblin:
                    enemiesKilld.GoblinsKilled++;
                    break;
                case NPCTypes.Troll:
                    enemiesKilld.TrollsKilled++;
                    break;
                default:
                    break;
            }
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

    public event Action<string> onNPCQuestAssign;
    public void NPCQuestAssign(string npcName)
    {
        if (onNPCQuestAssign != null)
        {
            onNPCQuestAssign(npcName);
        }
    }
}