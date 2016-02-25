using UnityEngine;
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
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)));
        if (selectedTile)
        {
            updateSelected(ref selectedTile);
        }
	}

    public void ToggleSideBar()
    {
        ShowSideBar = !ShowSideBar;
        updateSideBar();
    }

    public void TileSelected(Tile tile)
    {
        GameObject goTile = RefTileMap.FetchBlueprint(tile.Type);
        if (goTile)
        {
            selectedTile = Instantiate(goTile);
            updateSelected(ref selectedTile);
        }
    }

    private void updateSelected(ref GameObject go)
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));
        pos.z = 0.0f;
        go.transform.position = pos;
        go.transform.localScale = new Vector3(RefTileMap.TileSize, RefTileMap.TileSize, 1.0f);
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
}
