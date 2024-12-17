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

        LockCursor();
    }

    private void Update()
    {
        UpdateCursorState();
    }

    public void UpdateCursorState()
    {
        cursorIsShown = InventoryUI.Instance.isActive ||
                        DialogueManager.instance.dialogueIsPlaying ||
                        PauseMenu.instance.isPaused;

        if (cursorIsShown == UnityEngine.Cursor.visible)
            return;

        if (cursorIsShown)
        {
            UnLockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    public void LockCursor()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnLockCursor()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
}