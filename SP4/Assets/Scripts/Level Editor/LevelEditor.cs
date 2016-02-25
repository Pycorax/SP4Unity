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

    // Controls
    public KeyCode PlaceKey = KeyCode.Mouse1;
    public KeyCode RemoveKey = KeyCode.Mouse2;

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
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)));
        if (selectedTile)
        {
            updateSelected(ref selectedTile);

            Vector3 worldClickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // On place block click
            if (Input.GetMouseButton(0))
            {
                if (ShowSideBar && worldClickPoint.x < Camera.main.transform.position.x)
                {
                    placeTile(worldClickPoint);
                }
                else if (!ShowSideBar)
                {
                    placeTile(worldClickPoint);
                }
            }

            // On remove block click
            if (Input.GetMouseButton(1))
            {
                if (ShowSideBar && worldClickPoint.x < Camera.main.transform.position.x)
                {
                    removeTile(worldClickPoint);
                }
                else if (!ShowSideBar)
                {
                    removeTile(worldClickPoint);
                }
            }
        }
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

    private void placeTile(Vector3 pos)
    {
        MultiLayerTile multiTile = RefTileMap.FetchTile(pos);
        if (multiTile != null && multiTile.IsWalkable())
        {
            GameObject newTile = Instantiate(selectedTile);
            Vector2 tileIndex = RefTileMap.FetchTileIndex(pos);
            newTile.gameObject.SetActive(true);
            newTile.transform.position = RefTileMap.GenerateStartPos(RefTileMap.RowCount, RefTileMap.ColCount, (int)tileIndex.x, (int)tileIndex.y);
            newTile.transform.localScale = new Vector3(RefTileMap.TileSize, RefTileMap.TileSize, 1.0f);
            newTile.transform.parent = RefTileMap.transform;
            RefTileMap.AddToTiles(newTile.GetComponent<Tile>());
            multiTile.AddFront(newTile);
        }
    }

    private void removeTile(Vector3 pos)
    {
        MultiLayerTile multiTile = RefTileMap.FetchTile(pos);
        if (multiTile != null)
        {
            GameObject tileToDestroy = multiTile.RemoveTop();
            if (tileToDestroy)
            {
                Tile t = tileToDestroy.GetComponent<Tile>();
                RefTileMap.DestroyTile(ref t);
                //Destroy(tileToDestroy);
            }
        }
    }
}
