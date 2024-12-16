using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
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
        isPaused = false;
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.inputEvents.onClosePressed -= PausePressed;
    }

    void PausePressed()
    {
        if (InventoryUI.Instance.isActive)
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
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
