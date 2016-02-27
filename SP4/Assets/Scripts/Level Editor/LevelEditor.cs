﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEditor : MonoBehaviour
{
    // List of UI types that are required to resize and re-position
    enum UI_TYPE
    {
        UI_SIDEBAR,
        UI_SIDEBAR_CONTAINER,
        UI_OPEN_SIDEBAR,
        UI_CLOSE_SIDEBAR,
        NUM_UI,
    }
    
    public EditorTileMap RefTileMap;
    public Text MapName;
    public LevelEditorObjectiveDropdown RefObjective;

    // Controls
    /*public KeyCode PlaceKey = KeyCode.Mouse0;
    public KeyCode RemoveKey = KeyCode.Mouse1;*/

    // Side Bar
    public bool ShowSideBar = false;
    public float SideBarPadding = 10.0f;
    public RectTransform[] UI = new RectTransform[(int)UI_TYPE.NUM_UI];

    private GameObject selectedTile = null;


	// Use this for initialization
	void Start ()
    {
        GetComponent<CanvasScaler>().referenceResolution = ScreenData.GetScreenSize();
        updateSideBar();
        resizeUI();
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*float canvasWidth = GetComponent<RectTransform>().rect.width;
        float panelWidth = GetUI(UI_TYPE.UI_SIDEBAR).rect.width;
        Debug.Log(canvasWidth - panelWidth);*/
        //Debug.Log(GetUI(UI_TYPE.UI_CLOSE_SIDEBAR).position.x);

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
                if (ShowSideBar && Input.mousePosition.x < GetUI(UI_TYPE.UI_CLOSE_SIDEBAR).position.x)
                {
                    placeTile(worldClickPoint);
                }
                else if (!ShowSideBar && Input.mousePosition.x < GetUI(UI_TYPE.UI_OPEN_SIDEBAR).position.x)
                {
                    placeTile(worldClickPoint);
                }
            }
        }

        // On remove block click
        if (Input.GetMouseButtonDown(1))
        {
            if (ShowSideBar && worldClickPoint.x < Camera.main.transform.position.x)
            {
                removeTile(worldClickPoint);
            }
            else if (!ShowSideBar && Input.mousePosition.x < GetUI(UI_TYPE.UI_OPEN_SIDEBAR).position.x)
            {
                removeTile(worldClickPoint);
            }
        }
    }

    public void Save()
    {
        RefTileMap.Save(RefObjective.Objective);
    }

    public void ToggleSideBar()
    {
        ShowSideBar = !ShowSideBar;
        updateSideBar();
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

    private void updateSelected(ref GameObject go)
    {
        float tileSize = RefTileMap.TileSize;
        float scaleRatio = go.GetComponent<Tile>().ScaleRatio;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0.0f;
        go.transform.position = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
        go.transform.localScale = new Vector3(tileSize * scaleRatio, tileSize * scaleRatio, 1.0f);
    }

    private void updateSideBar()
    {
        //GetUI(UI_TYPE.UI_SIDEBAR).gameObject.SetActive(ShowSideBar);
        //GetUI(UI_TYPE.UI_CLOSE_SIDEBAR).gameObject.SetActive(ShowSideBar);
        GetUI(UI_TYPE.UI_SIDEBAR_CONTAINER).gameObject.SetActive(ShowSideBar);
        GetUI(UI_TYPE.UI_OPEN_SIDEBAR).gameObject.SetActive(!ShowSideBar);
    }

    private void resizeUI()
    {
        GetUI(UI_TYPE.UI_SIDEBAR);
    }

    private RectTransform GetUI(UI_TYPE type)
    {
        return UI[(int)type];
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
