using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager instance { get; private set; }
    public TextMeshProUGUI HintMessage;
    public float fadeDuration = .25f;
    public bool isActive { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if (HintMessage != null)
            HintMessage.text = string.Empty;
    }

    public void ShowHint(string message)
    {
        if (HintMessage != null)
        {
            isActive = true;
            StopAllCoroutines(); // Останавливаем все запущенные корутины, чтобы избежать конфликтов
            HintMessage.alpha = 0;
            HintMessage.text = message;
            StartCoroutine(FadeIn());
        }
    }

    public void HideHint()
    {
        if (HintMessage != null)
        {
            isActive = false;
            StopAllCoroutines(); // Останавливаем все запущенные корутины, чтобы избежать конфликтов
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            HintMessage.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        HintMessage.alpha = 1; // Убедимся, что альфа равна 1 в конце анимации
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            HintMessage.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        HintMessage.alpha = 0; // Убедимся, что альфа равна 0 в конце анимации
        HintMessage.text = string.Empty; // Очистка текста после полного исчезновения
    }
}


