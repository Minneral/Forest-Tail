using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Ссылка на AudioMixer для регулировки громкости
    public Slider volumeSlider;   // Слайдер для регулировки громкости
    public TMPro.TMP_Dropdown qualityDropdown; // Выпадающий список для выбора качества графики

    // Загружаем настройки при старте сцены
    void Start()
    {
        // Загружаем сохранённые настройки громкости
        float volume = PlayerPrefs.GetFloat("volume", 0.75f); // Значение по умолчанию 0.75
        volumeSlider.value = volume;
        SetVolume(volume); // Применяем сохранённый уровень громкости

        // Загружаем сохранённое качество графики
        int qualityIndex = PlayerPrefs.GetInt("quality", 2); // Значение по умолчанию - среднее качество
        qualityDropdown.value = qualityIndex;
        SetQuality(qualityIndex); // Применяем сохранённое качество
    }

    // Метод для изменения громкости
    public void SetVolume(float volume)
    {
        // Сохраняем значение громкости в PlayerPrefs
        PlayerPrefs.SetFloat("volume", volume);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20); // Настройка громкости через AudioMixer
    }

    // Метод для изменения качества графики
    public void SetQuality(int qualityIndex)
    {
        // Сохраняем выбранное качество в PlayerPrefs
        PlayerPrefs.SetInt("quality", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex); // Устанавливаем качество графики
    }

    // Метод для возврата в главное меню
    public void BackToMainMenu()
    {
        // Сцена с главным меню может быть загружена здесь
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
