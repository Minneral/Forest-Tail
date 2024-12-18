using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath += Die;
        currentHealth = maxHealth;
    }

    private void OnDestroy() {
        GameEventsManager.instance.npcEvents.onNPCDeath -= Die;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Текущее здоровье врага: " + currentHealth);

        if (currentHealth <= 0)
        {
            GameEventsManager.instance.npcEvents.NPCDeath();
        }
    }

    void Die()
    {
        Debug.Log("Враг умер.");
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
