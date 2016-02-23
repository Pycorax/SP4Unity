using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public Image Splash;

    private bool FadedIn = false;
    private bool FadedOut = false;

    public int FadeInSeconds = 2;
    public int FadeOutSeconds = 2;

	// Use this for initialization
	void Start () {
        //Set Alpha to zero for fade in
        Splash.canvasRenderer.SetAlpha(0f);
	}
	
	// Update is called once per frame
	void Update () {
        //Fade In
        if (Splash.canvasRenderer.GetAlpha() < 0.95f && FadedIn == false)
        {
            FadeIn();
            if(Splash.canvasRenderer.GetAlpha() >= 0.95f)
            {
                FadedIn = true;
            }
        }

        //Fade Out
        if (FadedIn == true && FadedOut == false)
        {
            FadeOut();
            if (Splash.canvasRenderer.GetAlpha() <= 0.10f)
            {
                FadedOut = true;
            }
        }
        
        //Load Next Scene when done
        if(FadedIn == true && FadedOut == true)
        {
            Application.LoadLevel("GameScene");
        }
	}
    
    //Fade In
    private void FadeIn()
    {    
        Splash.CrossFadeAlpha(1f, FadeInSeconds, false);
    }

    //Fade Out
    private void FadeOut()
    {
        Splash.CrossFadeAlpha(0f, FadeOutSeconds, false);
    }
}
