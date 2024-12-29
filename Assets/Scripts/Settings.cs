using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для TextMeshPro Dropdown

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Dropdown для выбора разрешения
    public Toggle fullscreenToggle;        // Переключатель полноэкранного режима
    public Slider brightnessSlider;        // Слайдер для регулировки яркости
    public Slider volumeSlider;            // Слайдер для регулировки громкости

    private Resolution[] uniqueResolutions;

    void Start()
    {
        // Инициализация настроек
        InitializeResolutions();
        LoadSettings();

        // Добавляем слушатели для слайдеров
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void InitializeResolutions()
    {
        // Получаем уникальные разрешения экрана
        uniqueResolutions = GetUniqueResolutions(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Length; i++)
        {
            string option = $"{uniqueResolutions[i].width} x {uniqueResolutions[i].height}";
            options.Add(option);

            // Определяем индекс текущего разрешения
            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    private Resolution[] GetUniqueResolutions(Resolution[] resolutions)
    {
        var uniqueResList = new System.Collections.Generic.List<Resolution>();
        foreach (var res in resolutions)
        {
            if (!uniqueResList.Exists(r => r.width == res.width && r.height == res.height))
            {
                uniqueResList.Add(res);
            }
        }
        return uniqueResList.ToArray();
    }

    private void LoadSettings()
    {
        // Загружаем сохранённые настройки
        fullscreenToggle.isOn = PlayerPrefs.GetInt("IsFullscreen", Screen.fullScreen ? 1 : 0) == 1;

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void ApplySettings()
    {
        // Применяем выбранное разрешение
        int resolutionIndex = resolutionDropdown.value;
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);

        // Сохраняем настройки
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("IsFullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void SetBrightness(float value)
    {
        RenderSettings.ambientLight = Color.white * value;
    }

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}

