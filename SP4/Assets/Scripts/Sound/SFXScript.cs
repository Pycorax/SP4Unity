using UnityEngine;
using UnityEngine.UI;

public class SFXScript : MonoBehaviour
{
    [Tooltip("A handle to the slider to load default values.")]
    public Slider SFXSlider;

	// Use this for initialization
	void Start ()
	{
	    SFXSlider.value = SoundManager.SFXVolume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeVolume(float volume)
    {
        Mathf.Clamp(volume, 0f, 1f);
        SoundManager.SFXVolume = volume;
    }
}
