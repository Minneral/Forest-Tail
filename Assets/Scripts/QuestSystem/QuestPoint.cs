using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    [Header("Quest Assignment")]
    [SerializeField] private bool assignByDialogue = true;
    [SerializeField] private bool assignBySubmit = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed += QuestAssignBySubmit;
        GameEventsManager.instance.npcEvents.onNPCQuestAssign += QuestAssignByDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        GameEventsManager.instance.inputEvents.onSubmitPressed -= QuestAssignBySubmit;
        GameEventsManager.instance.npcEvents.onNPCQuestAssign -= QuestAssignByDialogue;
    }

    void QuestAssignBySubmit()
    {
        if (assignBySubmit && !assignByDialogue)
        {
            QuestAssignPerform();
        }
    }

    void QuestAssignByDialogue(string npcName)
    {
        if (!assignBySubmit && assignByDialogue)
        {
            if (questInfoForPoint.NPCAssignerTag.Contains(npcName))
                QuestAssignPerform();
        }
    }


    private void QuestAssignPerform()
    {
        if (!playerIsNear)
        {
            return;
        }

        // start or finish a quest
        if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("Quest state: " + quest.state.ToString());
            // questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    void OnValidate()
    {
#if UNITY_EDITOR

        if (assignByDialogue)
            assignBySubmit = false;

        if (assignBySubmit)
            assignByDialogue = false;

#endif
    }
}