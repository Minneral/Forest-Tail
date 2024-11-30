using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int _health;
    private int _stamina;
    private int _armor;

    public int maxHealth = 100;
    public int maxStamina = 100;
    public int maxArmor = 100;

    public int defaultHealth = 100;
    public int defaultStamina = 100;
    public int defaultArmor = 0;

    public int staminaRegenerateValue = 5;
    public float staminaRegenerationDelay = 3f;

    private Coroutine _staminaRegenerationCoroutine;

    void Start()
    {
        _health = defaultHealth;
        _stamina = defaultStamina;
        _armor = defaultArmor;

        if (staminaRegenerationDelay < 1)
            staminaRegenerationDelay = 1;
    }

    public int GetHealth() => _health;
    public int GetStamina() => _stamina;
    public int GetArmor() => _armor;

    public void TakeDamage(int damage)
    {
        if (_health - damage <= 0)
        {
            _health = 0;
            Die();
        }
        else
        {
            _health -= damage;
        }
    }

    public void Heal(int amount)
    {
        _health = Mathf.Min(_health + amount, maxHealth);
    }

    public void TakeStamina(int amount)
    {
        _stamina = Mathf.Max(_stamina - amount, 0);

        if (_staminaRegenerationCoroutine != null)
        {
            StopCoroutine(_staminaRegenerationCoroutine);
        }

        _staminaRegenerationCoroutine = StartCoroutine(StaminaRegenerationTimer());
    }

    public void RegenerateStamina(int amount)
    {
        _stamina = Mathf.Min(_stamina + amount, maxStamina);
    }

    public void SetArmor(int value)
    {
        _armor = Mathf.Min(value, maxArmor);
    }

    public void Die()
    {
        Debug.Log("Player has died!");
    }

    private IEnumerator StaminaRegenerationTimer()
    {
        yield return new WaitForSeconds(staminaRegenerationDelay);

        while (_stamina < maxStamina)
        {
            RegenerateStamina(staminaRegenerateValue);
            yield return new WaitForSeconds(1f);
        }

        _staminaRegenerationCoroutine = null;
    }
}
