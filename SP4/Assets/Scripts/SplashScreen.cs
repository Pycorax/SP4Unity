using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    public Image splash;

	// Use this for initialization
	void Start () {
        //Set Alpha to zero for fade in
        splash.canvasRenderer.SetAlpha(0);
	}
	
	// Update is called once per frame
	void Update () {

        //Fade In
        splash.CrossFadeAlpha(255, 2, false);

        StartCoroutine(delay(2));

        splash.CrossFadeAlpha(0, 2, false);

	}
    
    //Delay for 2 Seconds
    IEnumerator delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
