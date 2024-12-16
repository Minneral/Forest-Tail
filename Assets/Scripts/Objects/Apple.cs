using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private void CollectApple()
    {
        gameObject.SetActive(false);
        GameEventsManager.instance.miscEvents.AppleCollected();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectApple();
        }
    }
}
