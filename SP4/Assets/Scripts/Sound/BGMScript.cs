using UnityEngine;
using UnityEngine.UI;

public class BGMScript : MonoBehaviour
{
    [Tooltip("A handle to the slider to load default values.")]
    public Slider BGMSlider;

    // Use this for initialization
    void Start()
    {
        BGMSlider.value = SoundManager.BGMVolume;
    }

    public void ChangeVolume(float volume)
    {
        Mathf.Clamp(volume, 0f, 1f);
        Debug.Log(volume);
        SoundManager.BGMVolume = volume;
    }
}
