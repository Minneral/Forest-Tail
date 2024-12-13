using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider slider; // Слайдер, отображающий прогресс загрузки
    public TextMeshProUGUI progressText; // Текст для отображения процентов загрузки

    private void Start() {
        LoadScene(3);
    }

    // Метод для начала загрузки сцены
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex)); // Асинхронная загрузка сцены
    }

    // Корутин для асинхронной загрузки
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex); // Загрузка сцены в фоновом режиме
        operation.allowSceneActivation = false; // Запрещаем активировать сцену сразу

        while (!operation.isDone) // Пока сцена не загружена
        {
            // Прогресс загрузки
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Прогресс от 0 до 1
            slider.value = progress; // Обновляем слайдер
            progressText.text = (progress * 100f).ToString("F0") + "%"; // Обновляем текст с процентами

            // Когда сцена загрузится на 90%, активируем её
            if (operation.progress >= 0.9f)
            {
                progressText.text = "Нажмите любую клавишу..."; // Сообщение о завершении загрузки
                if (Input.anyKeyDown) // Если нажата любая клавиша
                {
                    operation.allowSceneActivation = true; // Активируем сцену
                }
            }

            yield return null; // Ожидаем следующего кадра
        }
    }

}
