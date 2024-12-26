using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameDialogue : MonoBehaviour
{
    public static StartGameDialogue instance { get; private set; }
    [SerializeField] private TextAsset inkJSON;
    public static bool wasShown = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasShown)
            return;

        if (other.CompareTag("Player"))
        {
            wasShown = true;
            DialogueManager.instance.EnterDialogueMode(inkJSON, "Game");
        }
    }
}
