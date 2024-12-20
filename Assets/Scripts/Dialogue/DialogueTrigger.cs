using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    // [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string npcName;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        // visualCue.SetActive(false);
    }

    private void Start() {
        GameEventsManager.instance.inputEvents.onInteractPressed += OnInteract;
    }

    private void OnDestroy() {
        GameEventsManager.instance.inputEvents.onInteractPressed -= OnInteract;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            // visualCue.SetActive(true);
        }
        else
        {
            // visualCue.SetActive(false);
        }
    }

    void OnInteract()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            DialogueManager.instance.EnterDialogueMode(inkJSON, npcName);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}