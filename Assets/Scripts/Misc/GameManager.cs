using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public HashSet<string> EnemiesEliminated = new HashSet<string>();
    public HashSet<string> NotRestoringCollectedItem = new HashSet<string>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void EnemyEliminated(string id)
    {
        EnemiesEliminated.Add(id);
    }

    public void ItemCollected(string id)
    {
        NotRestoringCollectedItem.Add(id);
    }
}
