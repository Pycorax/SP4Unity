using UnityEngine;
using System.Collections;

public class CreditScreen : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Start the music
        SoundManager.PlayBackgroundMusic(SoundManager.BackgroundMusic.Credit_Music);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
