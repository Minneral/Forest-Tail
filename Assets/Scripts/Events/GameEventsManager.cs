using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public MiscEvents miscEvents;
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public NPCEvents npcEvents;
    public PuzzleEvents puzzleEvents;

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
        npcEvents = new NPCEvents();
        puzzleEvents = new PuzzleEvents();

        LockCursor();
    }

    private void Update()
    {
        UpdateCursorState();
    }

    public void UpdateCursorState()
    {
        bool shouldShowCursor = IsAnyUIVisible();

        if (shouldShowCursor != cursorIsShown)
        {
            cursorIsShown = shouldShowCursor;

            if (cursorIsShown)
            {
                UnLockCursor();
            }
            else
            {
                LockCursor();
            }
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

    public bool IsAnyUIVisible(params Type[] excludedTypes)
    {
        bool IsExcluded<T>(T instance) => excludedTypes.Contains(typeof(T));

        return (!IsExcluded(InventoryUI.Instance) && InventoryUI.Instance.isActive) ||
               (!IsExcluded(DialogueManager.instance) && DialogueManager.instance.dialogueIsPlaying) ||
               (!IsExcluded(PauseMenu.instance) && PauseMenu.instance.isPaused) ||
               (!IsExcluded(QuestPanelUI.instance) && QuestPanelUI.instance.isActive);
    }

    public bool IsOnlyUIVisible(params Type[] excludedTypes)
    {
        bool IsExcluded<T>(T instance) => excludedTypes.Contains(typeof(T));

        return (IsExcluded(InventoryUI.Instance) && InventoryUI.Instance.isActive) &&
               (IsExcluded(DialogueManager.instance) && DialogueManager.instance.dialogueIsPlaying) &&
               (IsExcluded(PauseMenu.instance) && PauseMenu.instance.isPaused) &&
               (IsExcluded(QuestPanelUI.instance) && QuestPanelUI.instance.isActive);
    }

}
