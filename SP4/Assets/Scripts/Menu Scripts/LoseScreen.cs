using UnityEngine;
using System.Collections;

public class LoseScreen : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Start the music
        SoundManager.PlayBackgroundMusic(SoundManager.BackgroundMusic.Stage_Fail);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
