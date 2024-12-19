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
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath += Die;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.npcEvents.onNPCDeath -= Die;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max(currentHealth - damage, 0);

        if (currentHealth <= 0)
        {
            GameEventsManager.instance.npcEvents.NPCDeath();
        }
    }

    void Die()
    {
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
