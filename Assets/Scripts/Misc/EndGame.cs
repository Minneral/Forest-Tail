using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (QuestManager.instance.IsQuestStateEqualOrGreater("KillTrollQuest", QuestState.FINISHED))
            {
                var objectsToDestroy = GameObject.FindObjectsOfType<DontDestroy>();
                foreach (DontDestroy obj in objectsToDestroy)
                {
                    Destroy(obj.gameObject);
                }
                GameEventsManager.instance.UnLockCursor();
                SceneManager.LoadScene("EndGame");
            }
        }
    }
}
