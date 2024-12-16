using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;

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

        // initialize all events
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        // goldEvents = new GoldEvents();
        miscEvents = new MiscEvents();
        questEvents = new QuestEvents();
    }
}