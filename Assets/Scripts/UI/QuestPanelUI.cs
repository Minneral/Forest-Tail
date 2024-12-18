using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    public GameObject hub;
    public Transform inProgressContent;
    public Transform completedContent;
    public bool isActive { get; private set; }
    public static QuestPanelUI instance { get; private set; }

    List<Transform> inProgressChilds;
    List<Transform> completedChilds;

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

        try
        {
            if (inProgressContent == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, "inProgressContent is null");

            if (completedContent == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, "completedContent is null");

            inProgressChilds = new List<Transform>();
            completedChilds = new List<Transform>();

            foreach (Transform child in inProgressContent)
            {
                inProgressChilds.Add(child);
                child.gameObject.SetActive(false);
            }

            foreach (Transform child in completedContent)
            {
                completedChilds.Add(child);
                child.gameObject.SetActive(false);
            }

            isActive = false;
            hub.SetActive(isActive);
        }
        catch (MissingComponentException ex)
        {
            Debug.Log(ex.Message);
            hub.SetActive(false);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onQuestMenuPressed += ToggleMenu;
        GameEventsManager.instance.inputEvents.onClosePressed += CloseMenu;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onQuestMenuPressed -= ToggleMenu;
        GameEventsManager.instance.inputEvents.onClosePressed -= CloseMenu;

    }

    void ToggleMenu()
    {
        isActive = !isActive;

        if (isActive)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    void OpenMenu()
    {
        if (PauseMenu.instance.isPaused ||
            InventoryUI.Instance.isActive ||
            DialogueManager.instance.dialogueIsPlaying)
            return;

        GameEventsManager.instance.UnLockCursor();

        isActive = true;
        hub.SetActive(isActive);
    }

    void CloseMenu()
    {
        GameEventsManager.instance.LockCursor();

        isActive = false;
        hub.SetActive(isActive);
    }

    void Update()
    {
        if (!isActive)
            return;

        var completedQuests = QuestManager.instance.GetCompletedQuests();
        var inProgressQuests = QuestManager.instance.GetInProgressQuests();

        UpdateViewContentUI(inProgressChilds, inProgressQuests);
        UpdateViewContentUI(completedChilds, completedQuests);
    }

    void UpdateViewContentUI(List<Transform> content, List<Quest> quests)
    {
        foreach (Transform item in content)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < quests.Count; i++)
        {
            content[i].gameObject.SetActive(true);
            var tuple = parseContentItem(content[i]);
            tuple.questName.text = quests[i].info.displayName;
            tuple.questDesciption.text = quests[i].info.description;
        }
    }

    (TextMeshProUGUI questName, TextMeshProUGUI questDesciption) parseContentItem(Transform item)
    {
        var questName = item.Find("QuestName").GetComponent<TextMeshProUGUI>();
        var questDescription = item.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

        return (questName, questDescription);
    }
}
