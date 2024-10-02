using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Settings : MonoBehaviour
{
    public float volume;

    [Header("OptionsUI")]
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDrowdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDrowdown.ClearOptions();

        List<string> res = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            res.Add(resolutions[i].width + " x " + resolutions[i].height);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDrowdown.value = i;
                resolutionDrowdown.RefreshShownValue();
            }
        }

        resolutionDrowdown.AddOptions(res);

    }

    private void Update()
    {
        volume = volumeSlider.value;
    }

    public void SetGraphics(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
