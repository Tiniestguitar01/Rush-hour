using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

public class Settings : MonoBehaviour
{
    public float volume;

    [Header("OptionsUI")]
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDrowdown;
    public TMP_Dropdown graphicsDropdown;
    public Toggle fullscreenToggle;

    Resolution[] resolutions;

    Database database;

    private void Start()
    {
        database = InstanceCreator.GetDatabase();

        resolutions = Screen.resolutions;

        resolutionDrowdown.ClearOptions();

        List<string> res = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            res.Add(resolutions[i].width + " x " + resolutions[i].height + " " + String.Format("{0:F2}", resolutions[i].refreshRateRatio) + " Hz");

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDrowdown.value = i;
                resolutionDrowdown.RefreshShownValue();
            }
        }

        resolutionDrowdown.AddOptions(res);

        GetSettings();

    }

    private void Update()
    {
        volume = volumeSlider.value;
    }

    public void GetSettings()
    {
        Dictionary<string, string> settings = database.settingHandler.GetUserSettings(database.loggedInUser);

        if (settings.ContainsKey("volume"))
        {
            volumeSlider.value = float.Parse(settings["volume"]);
        }
        else
        {
            volumeSlider.value = 0;
        }

        if (settings.ContainsKey("quality"))
        {
            SetGraphics(int.Parse(settings["quality"]));
        }
        else
        {
            SetGraphics(1);
        }

        if (settings.ContainsKey("resolution"))
        {
            int width = int.Parse(settings["resolution"].Split('x')[0]);
            int height = int.Parse(settings["resolution"].Split('x')[1]);
            int refreshRate = int.Parse(settings["resolution"].Split('x')[2]);
            int index = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == width &&
                    resolutions[i].height == height &&
                    resolutions[i].refreshRateRatio.value == refreshRate)
                {
                    index = i;
                }
            }
            SetResolution(index);
        }
        else
        {
            int index = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    index = i;
                }
            }
            SetResolution(index);
        }

        if (settings.ContainsKey("fullscreen"))
        {
            SetFullscreen(bool.Parse(settings["fullscreen"]));
        }
        else
        {
            SetFullscreen(true);
        }
    }

    public void SetVolume()
    {
        Setting setting = new Setting("volume", volumeSlider.value.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }

    public void SetGraphics(int quality)
    {
        graphicsDropdown.value = quality;
        graphicsDropdown.RefreshShownValue();
        QualitySettings.SetQualityLevel(quality);

        Setting setting = new Setting("quality", quality.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }

    public void SetResolution(int index)
    {
        resolutionDrowdown.value = index;
        resolutionDrowdown.RefreshShownValue();
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolutions[index].refreshRateRatio);

        Setting setting = new Setting("resolution", resolution.width + "x" + resolution.height + "x" + resolutions[index].refreshRateRatio, database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        Setting setting = new Setting("fullscreen", isFullscreen.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }
}
