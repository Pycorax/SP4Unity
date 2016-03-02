using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    public RectTransform LevelEditorPanel;
    public InputField MapWidth, MapHeight;
    private bool LevelEditorPanelView = false;

    public Text MapName;

	// Use this for initialization
	void Start ()
    {
        LevelEditorPanel.gameObject.SetActive(LevelEditorPanelView);
        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level), "");
	}
	
	// Update is called once per frame
	void Update ()
    {
        MapName.text = SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Level);
    }

    public void ToggleLevelEditorPanel()
    {
        LevelEditorPanelView = !LevelEditorPanelView;
        LevelEditorPanel.gameObject.SetActive(LevelEditorPanelView);
    }

    public void CreateNew()
    {
        if (MapWidth.text == "" || MapHeight.text == "")
        {
            return;
        }

        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level_Editor_Creation), 1);
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level_Editor_Row), Int32.Parse(MapWidth.text));
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level_Editor_Col), Int32.Parse(MapHeight.text));
        Application.LoadLevel("NewLevelEditor");
    }

    public void Load()
    {
        string name = SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Level);
        if (name == "")
        {
            return;
        }
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level_Editor_Creation), 0);
        Application.LoadLevel("NewLevelEditor");
    }
}
