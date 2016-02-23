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
    public void LoadGame1()
    {
        Application.LoadLevel("GameScene");
    }

    public void LoadGame2()
    {
        Application.LoadLevel("WhateverKeithWants");
    }

    public void LoadGame3()
    {
        Application.LoadLevel("Level3Scene");
    }

    public void LoadGame4()
    {
        Application.LoadLevel("Level4Scene");
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

    public void LoadLevelSelectScene()
    {
        Application.LoadLevel("LevelSelectScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
