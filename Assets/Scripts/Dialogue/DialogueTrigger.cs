using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    // [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        // visualCue.SetActive(false);
    }

    private void Start() {
        GameEventsManager.instance.inputEvents.onSubmitPressed += OnSubmit;
    }

    private void OnDestroy() {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= OnSubmit;
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

    void OnSubmit()
    {
        if (playerInRange && !DialogueManager.instance.dialogueIsPlaying)
        {
            DialogueManager.instance.EnterDialogueMode(inkJSON);
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