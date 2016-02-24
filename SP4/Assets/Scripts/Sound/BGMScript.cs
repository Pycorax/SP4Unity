using UnityEngine;
using System.Collections;

public class BGMScript : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeVolume(float volume)
    {
        Mathf.Clamp(volume, 0f, 1f);
        SoundManager.BGMVolume = volume;
    }
}
