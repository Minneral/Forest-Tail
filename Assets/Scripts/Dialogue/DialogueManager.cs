using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] public GameObject dialoguePanel;
    // [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private string lastNPCName;
    private string lastLine;
    public bool needUpdateVariables = false;
    private bool continueLocked = false;


    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    public static DialogueManager instance { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string QUEST_TAG = "quest";
    private const string PUZZLE_TAG = "puzzle";
    private DialogueVariables dialogueVariables;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Переинициализация объектов
        Initialize();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        dialoguePanel.SetActive(true);
        foreach (var choice in choices)
        {
            choice.SetActive(true);
        }
    }

    void Initialize()
    {
        dialoguePanel = GameObject.Find("DialogueUI");
        dialogueText = GameObject.Find("dialogueText").GetComponent<TextMeshProUGUI>();
        displayNameText = GameObject.Find("dialogueName").GetComponent<TextMeshProUGUI>();
        portraitAnimator = GameObject.Find("dialogueImage").GetComponent<Animator>();
        GameObject tchoices = GameObject.Find("Choices");

        List<GameObject> choiceChildren = new List<GameObject>();
        foreach (Transform child in tchoices.transform)
        {
            choiceChildren.Add(child.gameObject);
        }

        choiceChildren.Sort((GameObject go1, GameObject go2) => go1.name.CompareTo(go2.name));

        choices = choiceChildren.ToArray();

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

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

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
    }

    private void Start()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed += OnSubmit;
        GameEventsManager.instance.puzzleEvents.onMemoriesEnd += UnLockContinue;

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= OnSubmit;
        GameEventsManager.instance.puzzleEvents.onMemoriesEnd -= UnLockContinue;
    }

    void UnLockContinue()
    {
        continueLocked = false;
    }

    void OnSubmit()
    {
        // Если есть выборы
        if (currentStory.currentChoices.Count > 0)
        {
            return;
        }

        // Если текст полностью отображён
        if (canContinueToNextLine && !continueLocked)
        {
            ContinueStory();
        }
        // Если текст ещё печатается
        else if (!canContinueToNextLine)
        {
            SkipDisplayLine();
        }
    }


    public void EnterDialogueMode(TextAsset inkJSON, string npcName)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);
        lastNPCName = npcName;

        // reset portrait, layout, and speaker
        displayNameText.text = "???";

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);
        GameEventsManager.instance.npcEvents.NPCTalked(lastNPCName);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (needUpdateVariables)
        {
            needUpdateVariables = false;
            dialogueVariables.VariablesToStory(currentStory);
        }
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            // handle case where the last line is an external function
            if (nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            // handle case when dialogue line is empty
            if (nextLine.Equals(""))
            {
                ContinueStory();
                return;
            }
            // otherwise, handle the normal case for continuing the story
            else
            {
                // handle tags
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
                lastLine = nextLine;
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        // set the text to the full line, but set the visible characters to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        // hide items while text is typing
        // continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            // if not rich text, add the next letter and wait a small time
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // actions to take after the entire line has finished displaying
        // continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    void SkipDisplayLine()
    {
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine); // Прекращаем корутину
        }

        dialogueText.maxVisibleCharacters = lastLine.Length;
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        Debug.Log("Try to Handle tags");
        Debug.Log("Tags: " + currentTags.ToString());
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue.ToLower());
                    break;
                case QUEST_TAG:
                    GameEventsManager.instance.npcEvents.NPCQuestAssign(tagValue);
                    break;
                case PUZZLE_TAG:
                    GameEventsManager.instance.puzzleEvents.MemoriesStart();
                    continueLocked = true;
                    needUpdateVariables = true;
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            // NOTE: The below two lines were added to fix a bug after the Youtube video was made
            // GameEventsManager.instance.inputEvents.SubmitPressed();
            // InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        // dialogueVariables.variables.set
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    // This method will get called anytime the application exits.
    // Depending on your game, you may want to save variable state in other places.

    public void UpdateVariable(string variable_name, bool value)
    {
        dialogueVariables.UpdateVariable(variable_name, new Ink.Runtime.BoolValue(value));
    }

    public void OnApplicationQuit()
    {
        dialogueVariables.SaveVariables();
    }
}