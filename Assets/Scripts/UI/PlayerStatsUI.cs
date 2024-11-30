using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    private PlayerStats _stats;

    public Slider healthSlider;
    public Slider staminaSlider;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

            if (_stats == null)
                throw new MissingComponentException(nameof(PlayerStats), gameObject.name, GetType().Name, "You need to append 'PlayerStats' script");

            healthSlider.maxValue = _stats.maxHealth;
            staminaSlider.maxValue = _stats.maxStamina;
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }

        Debug.Log(_stats);
        Debug.Log(healthSlider);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        healthSlider.value = _stats.GetHealth();
        staminaSlider.value = _stats.GetStamina();
    }
}
