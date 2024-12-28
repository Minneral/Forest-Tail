using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    private static bool exitingCave = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void EnterCave()
    {
        SceneManager.LoadScene("Cave"); // Загрузка новой сцены
    }

    public void ExitCave()
    {
        exitingCave = true;
        SceneManager.LoadScene("World"); // Загрузка новой сцены
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.Find("Player");

        if (scene.name == "Cave")
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(10, 0, 5);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else if (scene.name == "World" && exitingCave)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(240, 20, 157);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
            exitingCave = false;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name != "Cave")
            {
                if (QuestManager.instance.IsQuestStateEqualOrGreater("KillTrollQuest", QuestState.CAN_START))
                {
                    EnterCave();
                }
                else
                {
                    HintManager.instance.ShowHint("На данный момент эта локация недоступна");
                }
            }
            else
            {
                ExitCave();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HintManager.instance.isActive)
            HintManager.instance.HideHint();
    }
}
