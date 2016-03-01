using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject HandleToSkinUI;

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
    //CANCER INCOMING
    public void LoadLevel()
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
        Application.LoadLevel("StoreScene");
    }

    public void LoadScores()
    {
        Application.LoadLevel("HighScoreScene");
    }

    public void LoadLevelSelectScene()
    {
        Application.LoadLevel("LevelSelectScene");
    }

    public void LoadLevelEditor()
    {
        Application.LoadLevel("NewLevelEditor");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowSkinUI()
    {
        HandleToSkinUI.SetActive(true);
    }

    public void HideSkinUI()
    {
        HandleToSkinUI.SetActive(false);
    }
}
