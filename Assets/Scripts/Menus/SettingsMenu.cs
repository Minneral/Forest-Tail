using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Slider mouseSensitivitySlider;

    private Resolution[] resolutions;

    void Start()
    {
        // Загрузка громкости
        float volume = PlayerPrefs.GetFloat("volume", 0.75f);
        volumeSlider.value = volume;
        SetVolume(volume);

        // Загрузка качества графики
        int qualityIndex = PlayerPrefs.GetInt("quality", 2);
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();
        SetQuality(qualityIndex);

        // Загрузка чувствительности мыши
        float mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 1.0f);
        mouseSensitivitySlider.value = mouseSensitivity;
        SetMouseSensitivity(mouseSensitivity);

        // Загрузка разрешения экрана
        resolutions = Screen.resolutions;
        List<string> resOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height} {resolutions[i].refreshRate}Hz";
            resOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resOptions);

        int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedResolutionIndex);

        // Назначение слушателей
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(SetVolume);

        qualityDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        mouseSensitivitySlider.onValueChanged.RemoveAllListeners();
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume); // Сохраняем значение
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex); // Сохраняем значение
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex); // Сохраняем значение
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("mouseSensitivity", sensitivity); // Сохраняем значение
        // Здесь можно добавить код для изменения чувствительности в скрипте управления
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
