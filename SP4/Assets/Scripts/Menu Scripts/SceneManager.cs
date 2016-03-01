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
    public void LoadLevel1()
    {
        Application.LoadLevel("Level1-1");
    }
    public void LoadLevel2()
    {
        Application.LoadLevel("Level1-2");
    }
    public void LoadLevel3()
    {
        Application.LoadLevel("Level1-3");
    }
    public void LoadLevel4()
    {
        Application.LoadLevel("Level2-1");
    }
    public void LoadLevel5()
    {
        Application.LoadLevel("Level2-2");
    }
    public void LoadLevel6()
    {
        Application.LoadLevel("Level2-3");
    }
    public void LoadLevel7()
    {
        Application.LoadLevel("Level3-1");
    }
    public void LoadLevel8()
    {
        Application.LoadLevel("Level3-2");
    }
    public void LoadLevel9()
    {
        Application.LoadLevel("Level3-3");
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
