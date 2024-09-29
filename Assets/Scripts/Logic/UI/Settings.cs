using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public float volume;

    [Header("OptionsUI")]
    public Slider volumeSlider;

    private void Update()
    {
        volume = volumeSlider.value;
    }
}
