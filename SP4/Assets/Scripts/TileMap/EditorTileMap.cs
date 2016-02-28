using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class EditorTileMap : TileMap
{
    private GameObject lineParent;
    public GameObject LineBlueprint;
    public float LineSize = 0.1f;
    public Color LineColor;
    public bool ShowLines = true;
    private float lineSizeRatio;

    public float ZoomSensitivity = 1.0f;
    private float pendingZoom = 0.0f;
    public float PendingZoom { set { pendingZoom = value; } get { return pendingZoom; } }

    private List<LineRenderer> gridLinesRow = new List<LineRenderer>();
    private List<LineRenderer> gridLinesCol = new List<LineRenderer>();

    // Components
    private WaypointManager waypointManager;

    // Use this for initialization
    protected override void Start ()
	{
        TotalSize = ScreenData.GetScreenSize();
        tileSize = calculateTileSize(TotalSize);
        base.Start();
        Load("Full_Map");
        /*CreateNew(50, 50);
        MultiLayerTile multiLayerTile = FetchTile(0, 0);
        GameObject temp = Instantiate(tileBlueprints[(int)Tile.TILE_TYPE.TILE_BARREL]);
        temp.SetActive(false);
        Vector3 newPos = generateStartPos(rowCount, colCount, 0, 0);
        newPos.z = 0.0f;
        temp.transform.position = newPos;
        temp.transform.localScale = new Vector3(tileSize, tileSize);
        temp.transform.parent = transform;
        tiles.Add(temp.GetComponent<Tile>());
        multiLayerTile.AddTile(temp);*/

        // Initialize Components
        waypointManager = GetComponent<WaypointManager>();
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (pendingZoom >= 1 || pendingZoom <= -1)
        {
            // Zooming
            int diff = Mathf.FloorToInt(pendingZoom);
            NumOfTiles -= diff;
            pendingZoom -= diff;
            NumOfTiles = Mathf.Clamp(NumOfTiles, 1, 50);
            updateMap();
        }
    }

    public override void Load(string name, int numOfTiles = 18)
    {
        base.Load(name, numOfTiles);
        lineSizeRatio = calculateLineSizeRatio();
    }

    public bool Save(Objectives.Type objective)
    {
        if (Name == "")
        {
            return false;
        }
        string path = MAP_DIRECTORY + Name + MAP_EXTENSION;
        StreamWriter file = new StreamWriter(File.Create(path));

        // Save objective
        file.WriteLine(TILE_OBJECTILE_IDENTIFIER.ToString() + ((int)objective));

        foreach (Row row in map)
        {
            string line = "";
            foreach (MultiLayerTile multiTile in row.column)
            {
                if (multiTile.multiLayerTile.Count > 0)
                {
                    foreach (GameObject goTile in multiTile.multiLayerTile)
                    {
                        Tile tile = goTile.GetComponent<Tile>();
                        line += (int)(tile.Type) + TILE_MULTIPLE_LAYER_SPLIT.ToString();
                    }
                    // Remove the extra multiple layer split
                    line = line.Remove(line.Length - 1);
                }
                else
                {
                    line += (int)Tile.TILE_TYPE.TILE_EMPTY;
                }
                line += TILE_SPLIT.ToString();
            }
            line = line.Remove(line.Length - 1);
            file.WriteLine(line);
        }
        file.Close();
        return true;
    }

    public void CreateNew(int numRow, int numCol)
    {
        rowCount = numRow;
        colCount = numCol;
        tileSize = calculateTileSize(TotalSize);
        tileMapDistToTopLeft = generateDistToTopLeft(numRow, numCol);
        
        // Calculate data needed
        Vector3 size = new Vector3(tileSize, tileSize, 1); // Size of tile (Scale)
        Vector3 startPos = generateStartPos(numRow, numCol); // Calculate start position

        for (int rowIndex = 0; rowIndex < numRow; ++rowIndex)
        {
            Row rowOfData = new Row();
            for (int colIndex = 0; colIndex < numCol; ++colIndex)
            {
                rowOfData.column.Add(new MultiLayerTile());
            }
            // Add row into map (Map is a list of rows)
            map.Add(rowOfData);
        }
        active = true;

        drawGridLines();
        lineSizeRatio = calculateLineSizeRatio();
    }

    public void ToggleLines()
    {
        ShowLines = !ShowLines;
        ActivateLines(ShowLines);
    }

    public void ToggleWaypointLines()
    {
        waypointManager.DrawConnections = !waypointManager.DrawConnections;
    }

    public void ActivateLines(bool active)
    {
        foreach (LineRenderer line in gridLinesRow)
        {
            line.gameObject.SetActive(active);
        }
        foreach (LineRenderer line in gridLinesCol)
        {
            line.gameObject.SetActive(active);
        }
    }

    private void drawGridLines()
    {
        if (!LineBlueprint)
        {
            return;
        }

        const int START = 0;
        const int END = 1;
        lineParent = new GameObject();
        lineParent.name = "Lines";
        lineParent.transform.parent = transform;

        // Draw lines between each row and 1 extra at the end
        for (int rowIndex = 0; rowIndex <= rowCount; ++rowIndex)
        {
            float y = TopBound - rowIndex * tileSize;
            Vector3[] points = new Vector3[2];
            points[START] = new Vector3(LeftBound, y, 1);
            points[END] = new Vector3(RightBound, y, 1);

            // Create lines
            LineRenderer line = Instantiate(LineBlueprint).GetComponent<LineRenderer>();
            line.SetPosition(START, points[START]);
            line.SetPosition(END, points[END]);
            line.SetWidth(tileSize * LineSize, tileSize * LineSize);
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.material.color = LineColor;
            line.SetColors(LineColor, LineColor);
            line.gameObject.SetActive(ShowLines);

            line.transform.parent = lineParent.transform;
            gridLinesRow.Add(line);
        }

        // Draw lines between each row and 1 extra at the end
        for (int colIndex = 0; colIndex <= colCount; ++colIndex)
        {
            float x = LeftBound + colIndex * tileSize;
            Vector3[] points = new Vector3[2];
            points[START] = new Vector3(x, TopBound, 1);
            points[END] = new Vector3(x, BottomBound, 1);

            // Create lines
            LineRenderer line = Instantiate(LineBlueprint).GetComponent<LineRenderer>();
            line.SetPosition(START, points[START]);
            line.SetPosition(END, points[END]);
            line.SetWidth(tileSize * LineSize, tileSize * LineSize);
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.material.color = LineColor;
            line.SetColors(LineColor, LineColor);
            line.gameObject.SetActive(ShowLines);

            line.transform.parent = lineParent.transform;
            gridLinesCol.Add(line);
        }
    }

    private void updateGridLines()
    {
        const int START = 0;
        const int END = 1;
        for (int rowIndex = 0; rowIndex < gridLinesRow.Count; ++rowIndex)
        {
            LineRenderer line = gridLinesRow[rowIndex];
            float y = TopBound - rowIndex * tileSize;
            Vector3[] points = new Vector3[2];
            points[START] = new Vector3(LeftBound, y, 1);
            points[END] = new Vector3(RightBound, y, 1);
            line.SetPosition(START, points[START]);
            line.SetPosition(END, points[END]);
            line.SetWidth(tileSize * LineSize, tileSize * LineSize);
        }
        for (int colIndex = 0; colIndex < gridLinesCol.Count; ++colIndex)
        {
            LineRenderer line = gridLinesCol[colIndex];
            float x = LeftBound + colIndex * tileSize;
            Vector3[] points = new Vector3[2];
            points[START] = new Vector3(x, TopBound, 1);
            points[END] = new Vector3(x, BottomBound, 1);
            line.SetPosition(START, points[START]);
            line.SetPosition(END, points[END]);
            line.SetWidth(tileSize * LineSize, tileSize * LineSize);
        }
    }

    private void updateMap()
    {
        // Map
        tileSize = calculateTileSize(TotalSize);
        tileMapDistToTopLeft = generateDistToTopLeft(rowCount, colCount);
        Vector3 startPos;

        for (int rowIndex = 0; rowIndex < map.Count; ++rowIndex)
        {
            Row row = map[rowIndex];
            for (int colIndex = 0; colIndex < row.column.Count; ++colIndex)
            {
                MultiLayerTile multiTile = row.column[colIndex];
                startPos = generateStartPos(rowCount, colCount, rowIndex, colIndex);
                for (int tileIndex = 0; tileIndex < multiTile.multiLayerTile.Count; ++tileIndex)
                {
                    GameObject tile = multiTile.multiLayerTile[tileIndex];
                    startPos.z = tile.transform.position.z;
                    float scaleRatio = tile.GetComponent<Tile>().ScaleRatio;
                    Vector3 size = new Vector3(tileSize * scaleRatio, tileSize * scaleRatio);
                    tile.transform.position = startPos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                    tile.transform.localScale = size;
                }
            }
        }

        // Grid lines
        updateGridLines();
    }

    private float calculateLineSizeRatio()
    {
        return LineSize / NumOfTiles;
    }

    public bool AddTile(Vector3 mousePos, GameObject blueprint)
    {
        MultiLayerTile multiTile = FetchTile(mousePos);
        Vector2 tileIndex = FetchTileIndex(mousePos);
        if (multiTile != null && multiTile.IsWalkable())
        {
            // Check if same tile exists
            foreach (GameObject tempGO in multiTile.multiLayerTile)
            {
                if (tempGO.GetComponent<Tile>().Type == blueprint.GetComponent<Tile>().Type)
                {
                    return false;
                }
            }

            float scaleRatio = blueprint.GetComponent<Tile>().ScaleRatio;
            List<MultiLayerTile> tempList = new List<MultiLayerTile>();

            // Set other tiles walkable to false if scale ratio bigger than 0
            for (int row = (int)tileIndex.x; row < tileIndex.x + scaleRatio; ++row)
            {
                for (int col = (int)tileIndex.y; col < tileIndex.y + scaleRatio; ++col)
                {
                    MultiLayerTile tile = FetchTile(row, col);
                    if (tile != null && tile.IsWalkable())
                    {
                        tile.Walkable = blueprint.GetComponent<Tile>().IsWalkable();
                        tempList.Add(tile);
                    }
                    else
                    {
                        foreach (MultiLayerTile t in tempList)
                        {
                            t.Walkable = t.CalculateWalkable();
                        }
                        return false;
                    }
                }
            }

            // Adding the tile
            //GameObject newTile = Instantiate(blueprint);
            Tile.TILE_TYPE type = blueprint.GetComponent<Tile>().Type;
            Vector3 pos = generateStartPos(RowCount, ColCount, (int)tileIndex.x, (int)tileIndex.y) + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
            Vector3 size = new Vector3(TileSize * scaleRatio, TileSize * scaleRatio, 1.0f);
            GameObject newTile = createTile(type, pos, size);

            /*newTile.transform.position = pos + ;
            newTile.transform.localScale = size;
            newTile.transform.parent = transform;*/
            tiles.Add(newTile.GetComponent<Tile>());
            multiTile.AddFront(newTile);
            return true;
        }
        return false;
    }

    public bool RemoveTile(Vector3 pos)
    {
        MultiLayerTile multiTile = FetchTile(pos);
        if (multiTile != null)
        {
            GameObject tileToDestroy = multiTile.RemoveTop();
            if (tileToDestroy)
            {
                Tile t = tileToDestroy.GetComponent<Tile>();
                DestroyTile(ref t);

                // Recalculate the walkable value for tiles occupied
                Vector2 tileIndex = FetchTileIndex(pos);
                float scaleRatio = t.ScaleRatio;
                for (int row = (int)tileIndex.x; row < tileIndex.x + scaleRatio; ++row)
                {
                    for (int col = (int)tileIndex.y; col < tileIndex.y + scaleRatio; ++col)
                    {
                        MultiLayerTile tile = FetchTile(row, col);
                        if (tile != null)
                        {
                            tile.Walkable = tile.CalculateWalkable();
                        }
                    }
                }

                return true;
            }
        }
        return false;
    }
}
