using UnityEngine;

public class PauseMenuScript : MonoBehaviour {

    public KeyCode pauseButton = KeyCode.Escape;
    private bool isPaused = false;
    public GameObject PausePanel;
	// Use this for initialization
	void Start () {
        PausePanel.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(pauseButton) && !isPaused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(pauseButton) && isPaused)
        {
            Resume();
        }
	}

    public void Resume()
    {
        PausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Pause()
    {
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void RestartRound()
    {
        Resume();
        Application.LoadLevel("GameScene");
    }

    public void ReturnToMainMenu()
    {
        Resume();
        Application.LoadLevel("MainMenuScene");
    }
}
