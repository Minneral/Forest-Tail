using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    public int maxHealth = 100;
    public bool isDead = false;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Отображаем текущее здоровье
        Debug.Log("Текущее здоровье врага: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Враг умер.");
        isDead = true;
        // Здесь можно добавить логику для смерти, например, проиграть анимацию или уничтожить объект

        // StartCoroutine(WaitAndExecute(1.0f, () => Destroy(this.gameObject)));
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private IEnumerator WaitAndExecute(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
