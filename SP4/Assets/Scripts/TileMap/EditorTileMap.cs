using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class EditorTileMap : TileMap
{
    public GameObject LineBlueprint;
    public float LineSize;
    public Color LineColor;
    private List<LineRenderer> gridLines = new List<LineRenderer>();

	// Use this for initialization
	protected override void Start ()
	{
        TotalSize = ScreenData.GetScreenSize();
        tileSize = calculateTileSize(TotalSize);
        base.Start();
        CreateNew(50, 50);
        MultiLayerTile multiLayerTile = FetchTile(0, 0);
        GameObject temp = Instantiate(tileBlueprints[(int)Tile.TILE_TYPE.TILE_BARREL]);
        temp.SetActive(false);
        temp.transform.position = generateStartPos(rowCount, colCount, 0, 0);
        temp.transform.localScale = new Vector3(tileSize, tileSize);
        temp.transform.parent = transform;
        tiles.Add(temp.GetComponent<Tile>());
        multiLayerTile.AddTile(temp);
    }
	
	// Update is called once per frame
	void Update ()
    {
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

        drawGridLines();
    }

    public void ActivateLines(bool active)
    {
        foreach (LineRenderer line in gridLines)
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

        // Draw lines between each row and 1 extra at the end
        for (int rowIndex = 0; rowIndex <= rowCount; ++rowIndex)
        {
            const int START = 0;
            const int END = 1;
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

            gridLines.Add(line);
        }

        // Draw lines between each row and 1 extra at the end
        for (int colIndex = 0; colIndex <= colCount; ++colIndex)
        {
            const int START = 0;
            const int END = 1;
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

            gridLines.Add(line);
        }
    }
}
