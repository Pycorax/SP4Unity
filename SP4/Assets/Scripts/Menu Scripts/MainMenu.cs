using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public RectTransform InputNamePanel;
    private bool InputNamePanelView = false;

    public InputField PlayerName;

	// Use this for initialization
	void Start ()
    {
        InputNamePanel.gameObject.SetActive(InputNamePanelView);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ToggleInputName()
    {
        InputNamePanelView = !InputNamePanelView;
        InputNamePanel.gameObject.SetActive(InputNamePanelView);
    }

    public void LevelSelect()
    {
        string name = PlayerName.text;
        if (name == "")
        {
            return;
        }
        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Name), name);
        Application.LoadLevel("LevelSelectScene");
    }
}
