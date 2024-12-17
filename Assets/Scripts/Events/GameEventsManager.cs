using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;

    private bool cursorIsShown = false;

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
        miscEvents = new MiscEvents();
        questEvents = new QuestEvents();

        Initialize();
    }

    void Initialize()
    {
        LockCursor();

        inputEvents.onInventoryPressed += ToggleCursor;
        inputEvents.onClosePressed += ToggleCursor;
        inputEvents.onInteractPressed += ToggleCursor;
    }

    void ToggleCursor()
    {
        cursorIsShown = InventoryUI.Instance.isActive ||
                        DialogueManager.instance.dialogueIsPlaying ||
                        PauseMenu.instance.isPaused;

        if (cursorIsShown)
        {
            // UnLockCursor();
            LockCursor();
        }
        else
        {
            UnLockCursor();
            // LockCursor();
        }
    }

    void LockCursor()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void UnLockCursor()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
}