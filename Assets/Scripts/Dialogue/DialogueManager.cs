using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Dialogue;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public Image dialogueImage;
    private Queue<DialogueLine> lines;
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        lines = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!InventoryUI.Instance.isActive)
        {
            dialogueBox.SetActive(true); // Показываем панель диалога

            lines.Clear();

            foreach (DialogueLine line in dialogue.lines)
            {
                this.lines.Enqueue(line);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        dialogueImage.sprite = line.characterIcon;
        nameText.text = line.characterName;
        string sentence = line.dialogueText;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }


    void EndDialogue()
    {
        dialogueBox.SetActive(false); // Скрываем панель диалога
    }


    public void NextSentence()
    {
        DisplayNextSentence();
    }


    IEnumerator<WaitForEndOfFrame> TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForEndOfFrame(); // Ожидание конца кадра для эффекта печати
        }
    }


}
