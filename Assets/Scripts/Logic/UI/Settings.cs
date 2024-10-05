using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class Settings : MonoBehaviour
{
    public float volume;

    [Header("OptionsUI")]
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDrowdown;

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
            res.Add(resolutions[i].width + " x " + resolutions[i].height);

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

        if (settings.ContainsKey("Volume"))
        {
            volumeSlider.value = float.Parse(settings["volume"]);
        }

        if (settings.ContainsKey("quality"))
        {
            QualitySettings.SetQualityLevel(int.Parse(settings["quality"]));
        }

        if (settings.ContainsKey("resolution"))
        {
            int width = int.Parse(settings["resolution"].Split('x')[0]);
            int height = int.Parse(settings["resolution"].Split('x')[1]);
            Screen.SetResolution(width, height, Screen.fullScreen);
        }

        if (settings.ContainsKey("fullscreen"))
        {
            Screen.fullScreen = bool.Parse(settings["fullscreen"]);
        }
    }

    public void SetVolume()
    {
        Setting setting = new Setting("volume", volumeSlider.value.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }

    public void SetGraphics(int quality)
    {
        QualitySettings.SetQualityLevel(quality);

        Setting setting = new Setting("quality", quality.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);

    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Setting setting = new Setting("resolution", resolution.width + "x" + resolution.height,database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        Setting setting = new Setting("fullscreen", isFullscreen.ToString(), database.loggedInUser.id);
        database.settingHandler.AddSetting(setting);
    }
}
