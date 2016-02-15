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
        Application.LoadLevel("MainMenu");
    }

    public void LoadGame()
    {
        Application.LoadLevel("Game");
    }

    public void LoadOptions()
    {
        Application.LoadLevel("Options");
    }

    public void LoadCredits()
    {
        Application.LoadLevel("Credits");
    }

    public void LoadShops()
    {
        Application.LoadLevel("Shops");
    }

    public void LoadScores()
    {
        Application.LoadLevel("Scores");
    }
}
