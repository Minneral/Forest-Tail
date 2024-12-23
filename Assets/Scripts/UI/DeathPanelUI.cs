using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanelUI : MonoBehaviour, IScreen
{
    public GameObject hub; // Ссылка на объект панели
    public float displayDelay = 2f; // Задержка перед показом панели
    private bool isActive = false;
    public static DeathPanelUI instance;

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

    private void OnEnable()
    {
        if (hub != null)
        {
            isActive = false;
            hub.SetActive(false);
        }

        GameEventsManager.instance.playerEvents.onPlayerDeath += DisplayScreen;
        GameEventsManager.instance.uiEvents.onCloseAllScreens += CloseScreen;
    }

    private void OnDisable()
    {
        if (hub != null)
        {
            isActive = false;
            hub.SetActive(false);
        }

        GameEventsManager.instance.playerEvents.onPlayerDeath -= DisplayScreen;
        GameEventsManager.instance.uiEvents.onCloseAllScreens -= CloseScreen;
    }

    public void LoadMainScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void CloseScreen()
    {
        isActive = false;
        hub.SetActive(false);
    }

    public void DisplayScreen()
    {
        GameEventsManager.instance.uiEvents.CloseAllScreens();
        if (hub != null)
        {
            StopAllCoroutines(); // Останавливаем предыдущие корутины
            StartCoroutine(DisplayScreenWithDelay());
        }
    }

    private IEnumerator DisplayScreenWithDelay()
    {
        yield return new WaitForSeconds(displayDelay); // Задержка
        isActive = true;
        hub.SetActive(true);
    }

    public bool IsActive()
    {
        return isActive;
    }
}
