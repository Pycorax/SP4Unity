using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour
{
    // Level editor panel
    public RectTransform LevelEditorPanel;
    public InputField MapWidth, MapHeight;
    private bool LevelEditorPanelView = false;

    // Weapon selection panel
    public RectTransform WeaponSelectionPanel;
    public ToggleGroup Player1Left, Player1Right, Player2Left, Player2Right;
    private bool WeaponSelectionPanelView = false;

    public Text MapName;

	// Use this for initialization
	void Start ()
    {
        LevelEditorPanel.gameObject.SetActive(LevelEditorPanelView);
        WeaponSelectionPanel.gameObject.SetActive(WeaponSelectionPanelView);
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

    public void ToggleWeaponSelectionPanel()
    {
        WeaponSelectionPanelView = !WeaponSelectionPanelView;
        WeaponSelectionPanel.gameObject.SetActive(WeaponSelectionPanelView);
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

    public void LoadLevelEditor()
    {
        string name = SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Level);
        if (name == "")
        {
            return;
        }
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level_Editor_Creation), 0);
        Application.LoadLevel("NewLevelEditor");
    }

    public void StartGame()
    {
        // Player 1 Left
        IEnumerator<Toggle> activeToggle = Player1Left.ActiveToggles().GetEnumerator();
        activeToggle.MoveNext();
        Toggle currentToggle = activeToggle.Current;
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player1_Left), (int)currentToggle.GetComponent<WeaponIdentifier>().Type);

        // Player 1 Right
        activeToggle = Player1Right.ActiveToggles().GetEnumerator();
        activeToggle.MoveNext();
        currentToggle = activeToggle.Current;
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player1_Right), (int)currentToggle.GetComponent<WeaponIdentifier>().Type);

        // Player 2 Left
        activeToggle = Player2Left.ActiveToggles().GetEnumerator();
        activeToggle.MoveNext();
        currentToggle = activeToggle.Current;
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player2_Left), (int)currentToggle.GetComponent<WeaponIdentifier>().Type);

        // Player 2 Right
        activeToggle = Player2Right.ActiveToggles().GetEnumerator();
        activeToggle.MoveNext();
        currentToggle = activeToggle.Current;
        PlayerPrefs.SetInt(SaveClass.GetKey(SaveClass.Save_Keys.Key_Player2_Right), (int)currentToggle.GetComponent<WeaponIdentifier>().Type);

        string name = SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Level);
        if (name == "")
        {
            return;
        }
        Application.LoadLevel("GameScene");
    }
}
