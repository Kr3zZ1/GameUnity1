using UnityEngine;
using UnityEngine.UI;
using TMPro; // ��� TextMeshPro Dropdown

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; // Dropdown ��� ������ ����������
    public Toggle fullscreenToggle;        // ������������� �������������� ������
    public Slider brightnessSlider;        // ������� ��� ����������� �������
    public Slider volumeSlider;            // ������� ��� ����������� ���������

    private Resolution[] uniqueResolutions;

    void Start()
    {
        // ������������� ��������
        InitializeResolutions();
        LoadSettings();

        // ��������� ��������� ��� ���������
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void InitializeResolutions()
    {
        // �������� ���������� ���������� ������
        uniqueResolutions = GetUniqueResolutions(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Length; i++)
        {
            string option = $"{uniqueResolutions[i].width} x {uniqueResolutions[i].height}";
            options.Add(option);

            // ���������� ������ �������� ����������
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
        // ��������� ���������� ���������
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
        // ��������� ��������� ����������
        int resolutionIndex = resolutionDropdown.value;
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);

        // ��������� ���������
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

