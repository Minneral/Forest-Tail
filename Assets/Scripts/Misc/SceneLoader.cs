using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static Vector3 lastPosition;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindWithTag("Player");
        if (scene.name == "Cave")
        {
            if (player != null)
            {
                player.transform.position = new Vector3(0, 0, 0);
            }
        }
        else if (scene.name == "World")
        {
            if (player != null)
            {
                // player.transform.position = lastPosition;
            }
        }
    }

    public void EnterCave()
    {
        lastPosition = GameObject.FindWithTag("Player").transform.position;
        SceneManager.LoadScene("Cave"); // Загрузка новой сцены
    }

    public void ExitCave()
    {
        SceneManager.LoadScene("World"); // Загрузка новой сцены
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name != "Cave")
                EnterCave();
            else
                ExitCave();
        }
    }
}
