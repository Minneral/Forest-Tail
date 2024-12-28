using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, IScreen
{
    public GameObject pauseMenuUI;
    public bool isPaused { get; private set; }

    public static PauseMenu instance;

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
    }

    private void Start()
    {
        GameEventsManager.instance.inputEvents.onClosePressed += PausePressed;
        GameEventsManager.instance.uiEvents.onCloseAllScreens += CloseScreen;
        isPaused = false;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.inputEvents.onClosePressed -= PausePressed;
        GameEventsManager.instance.uiEvents.onCloseAllScreens -= CloseScreen;
    }

    void PausePressed()
    {
        // Dont forget to set Script Execution Order!
        // And don't use subscribe in onEnable method
        if (GameEventsManager.instance.IsAnyUIVisible(typeof(PauseMenu), typeof(DialogueManager)))
            return;

        if (isPaused) ResumeGame();
        else
        {
            PauseGame();
        }

    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        var objectsToDestroy = GameObject.FindObjectsOfType<DontDestroy>();
        foreach (DontDestroy obj in objectsToDestroy)
        {
            Destroy(obj.gameObject);
        }
        StartGameDialogue.wasShown = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool IsActive()
    {
        return isPaused;
    }

    public void DisplayScreen()
    {
        PausePressed();
    }

    public void CloseScreen()
    {
        ResumeGame();
    }
}
