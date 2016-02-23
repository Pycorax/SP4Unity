using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public Image Splash;
    public float Alpha;

    bool FadedIn = false;
    bool FadedOut = false;

	// Use this for initialization
	void Start () {
        //Set Alpha to zero for fade in
        Splash.canvasRenderer.SetAlpha(0f);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Splash.canvasRenderer.GetAlpha());
        if (Splash.canvasRenderer.GetAlpha() < 0.95f && FadedIn == false)
        {
            Debug.Log("FadeIn");
            FadeIn();
            if(Splash.canvasRenderer.GetAlpha() >= 0.95f)
            {
                FadedIn = true;
            }
        }
        if (FadedIn == true && FadedOut == false)
        {
            Debug.Log("FadeOut");
            FadeOut();
            if (Splash.canvasRenderer.GetAlpha() <= 0.05f)
            {
                FadedOut = true;
            }
        }
        
        if(FadedIn == true && FadedOut == true)
        {
            Debug.Log("ayylmao");
            Application.LoadLevel("GameScene");
        }
	}
    
    //Delay for 2 Seconds
    IEnumerator delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    
    //Fade In
    private void FadeIn()
    {    
        Splash.CrossFadeAlpha(1f, 2, false);
    }

    //Fade Out
    private void FadeOut()
    {
        Splash.CrossFadeAlpha(0f, 2, false);
    }
}
