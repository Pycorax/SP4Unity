using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class LevelEditorGemini : MonoBehaviour
{
    public EditorTileMap RefTileMap;
    public InputField MapName;
    public LevelEditorObjectiveDropdown RefObjective;
    public InputField RefObjectiveParam;

    private GameObject selectedTile = null;

    // Components
    private GraphicRaycaster graphicRaycaster;

	// Use this for initialization
	void Start ()
    {
        // Set the screen size
        GetComponent<CanvasScaler>().referenceResolution = ScreenData.GetScreenSize();

        // Initialize Components
	    graphicRaycaster = GetComponent<GraphicRaycaster>();

        int creation = SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Level_Editor_Creation);
        if (creation == 1)
        {
            RefTileMap.CreateNew(SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Level_Editor_Row), SaveClass.GetPlayerPrefInt(SaveClass.Save_Keys.Key_Level_Editor_Col));
        }
        else if (creation == 0)
        {
            RefTileMap.Load(SaveClass.GetPlayerPrefString(SaveClass.Save_Keys.Key_Level));

            // Assign current values to editor UI
            MapName.text = RefTileMap.Name;
            var parameters = RefTileMap.ObjectiveParams.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            RefObjectiveParam.text = parameters[0];
        }
        else
        {
            Application.LoadLevel("MainMenuScene");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

        // Do not allow modification of the tilemap when the mouse isn't on it
	    if (mouseIsOnUi())
	    {
	        return;
	    }

	    // Update tile map scrolling
        if (RefTileMap)
	    {
	        RefTileMap.PendingZoom += Input.GetAxis("Mouse ScrollWheel") * RefTileMap.ZoomSensitivity;
	    }

	    // Update selected tile and check for tile placement
	    Vector3 worldClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Mouse position in world space
	    if (selectedTile)
	    {
	        updateSelected(ref selectedTile);

	        // On place tile click
	        if (Input.GetMouseButton(0))
	        {
	            placeTile(worldClickPoint);
	        }
	    }

	    // On remove block click
	    if (Input.GetMouseButtonDown(1))
        {
            removeTile(worldClickPoint);
        }
    }

    public void Save()
    {
        RefTileMap.Save(RefObjective.Objective, RefObjectiveParam.text);
    }

    public void TileSelected(Tile tile)
    {
        if (selectedTile)
        {
            Destroy(selectedTile);
            selectedTile = null;
        }
        GameObject goTile = RefTileMap.FetchBlueprint(tile.Type);
        if (goTile)
        {
            selectedTile = Instantiate(goTile);
            updateSelected(ref selectedTile);
        }
    }

    public void UpdateName(Text text)
    {
        RefTileMap.Name = text.text;
    }

    public void Exit()
    {
        Application.LoadLevel("LevelSelectScene");
    }

    private bool mouseIsOnUi()
    {
        // Create a container to store PED to pass into the raycaster
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;

        //Create list to store list of collisions
        List<RaycastResult> collisions = new List<RaycastResult>();
        
        //Raycast it
        graphicRaycaster.Raycast(ped, collisions);

        bool result = collisions.Count > 0;

        return result;
    }

    private void updateSelected(ref GameObject go)
    {
        float tileSize = RefTileMap.TileSize;
        float scaleRatio = go.GetComponent<Tile>().ScaleRatio;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        go.transform.position = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
        go.transform.localScale = new Vector3(tileSize * scaleRatio, tileSize * scaleRatio, 1.0f);
    }

    private void placeTile(Vector3 pos)
    {
        RefTileMap.AddTile(pos, selectedTile);
    }

    private void removeTile(Vector3 pos)
    {
        RefTileMap.RemoveTile(pos);
    }
}
