using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public Image Splash;
    public float Alpha;

	// Use this for initialization
	void Awake () {
        //Set Alpha to zero for fade in
        Splash.canvasRenderer.SetAlpha(0f);
        Alpha = Splash.canvasRenderer.GetAlpha();
	}
	
	// Update is called once per frame
	void Update () {

        if(Alpha < 1f)
        {
            FadeIn();
        }
        else if(Alpha >= 1f)
        {
            FadeOut();
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
