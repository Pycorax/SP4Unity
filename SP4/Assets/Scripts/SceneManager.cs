using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadMainMenu()
    {
        Application.LoadLevel("MainMenuScene");
    }

    //Load Up GameScene
    public void LoadGame()
    {
        Application.LoadLevel("GameScene");
    }

    public void LoadOptions()
    {
        Application.LoadLevel("OptionScene");
    }

    public void LoadCredits()
    {
        Application.LoadLevel("CreditScene");
    }

    public void LoadShops()
    {
        Application.LoadLevel("ShopScene");
    }

    public void LoadScores()
    {
        Application.LoadLevel("ScoreScenes");
    }
}
