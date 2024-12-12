using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public Sprite characterIcon;
        public string characterName;
        [TextArea(2, 5)] public string dialogueText;
    }

    public DialogueLine[] lines;
}
