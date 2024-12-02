using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    private PlayerStats _stats;

    public Slider healthSlider;
    public Slider staminaSlider;

    // These will store the target values for health and stamina sliders
    private float targetHealthValue;
    private float targetStaminaValue;

    // These will store the time taken for the sliders to move towards their target values
    public float transitionSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                throw new MissingComponentException(nameof(Inventory), gameObject.name, GetType().Name, "You need to assign tag 'Player'");

            _stats = player.GetComponent<PlayerStats>();

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

        // Initializing target values
        targetHealthValue = _stats.GetHealth();
        targetStaminaValue = _stats.GetStamina();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        targetHealthValue = _stats.GetHealth();

        targetStaminaValue = _stats.GetStamina();

        healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealthValue, Time.deltaTime * transitionSpeed);
        staminaSlider.value = Mathf.Lerp(staminaSlider.value, targetStaminaValue, Time.deltaTime * transitionSpeed);
    }
}
