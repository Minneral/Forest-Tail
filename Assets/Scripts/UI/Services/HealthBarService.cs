using UnityEngine;
using UnityEngine.UI;

public class HealthBarService : MonoBehaviour
{
    [Header("Required")]
    public Transform target; // Объект, над которым будет слайдер
    [Header("Optional")]
    public Transform playerCamera; // Ссылка на камеру игрока
    public float transitionSpeed = 5;
    public Vector3 offset = new Vector3(0, 2.0f, 0); // Смещение над головой объекта
    private Slider healthSlider;
    private NPCStats stats;

    private void Awake()
    {
        try
        {
            healthSlider = GetComponentInChildren<Slider>();

            if (playerCamera == null)
                playerCamera = Camera.main.transform;

            if (target == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name);

            if (healthSlider == null)
                throw new MissingComponentException(nameof(Slider), gameObject.name, GetType().Name);

            stats = target.GetComponent<NPCStats>();
            if (stats == null)
                throw new MissingComponentException(nameof(NPCStats), gameObject.name, GetType().Name);

            healthSlider.maxValue = stats.maxHealth;
        }
        catch (MissingComponentException ex)
        {
            Debug.Log(ex.Message);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (playerCamera != null)
        {
            // Поворот слайдера в сторону камеры
            healthSlider.transform.rotation = Quaternion.LookRotation(healthSlider.transform.position - playerCamera.position);

            // Обновление позиции слайдера над головой цели с учетом смещения
            Vector3 targetPosition = target.position + offset;
            healthSlider.transform.position = targetPosition;
        }

        SetHealth(stats.GetHealth());
    }

    public void SetHealth(float health)
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, Time.deltaTime * transitionSpeed);
    }

    public void DestroyService()
    {
        // Найти Canvas, который содержит healthSlider
        Canvas canvas = healthSlider.GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            Destroy(canvas.gameObject); // Удалить весь объект Canvas
        }

        Destroy(this); // Удалить саму службу HealthBarService
    }

}
