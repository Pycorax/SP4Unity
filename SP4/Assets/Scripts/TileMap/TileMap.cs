using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/**********
Multi-layer tile system
NOTE: Writing format is from top to bottom. No spaces allowed
Example: Tile A is supposed to be rendered above Tile B, writing format is "A|B"
**********/

public class MultiLayerTile
{
	public List<GameObject> multiLayerTile = new List<GameObject>();
    private bool walkable = true;

	public bool IsWalkable()
	{
        return walkable;
	}

    public void AddTile(GameObject tile)
    {
        multiLayerTile.Add(tile);
        walkable = calculateWalkable();
    }

    private bool calculateWalkable()
    {
        foreach (GameObject tile in multiLayerTile)
        {
            if (!tile.GetComponent<Tile>().IsWalkable())
            {
                return false;
            }
        }
        return true;
    }
}

public class Row
{
	public List<MultiLayerTile> column = new List<MultiLayerTile>();
}

public abstract class TileMap : MonoBehaviour
{
	public static char TILE_SPLIT = ',';
	public static char TILE_MULTIPLE_LAYER_SPLIT = '|';
    public static string MAP_DIRECTORY = "Assets\\Maps\\";
    public static string MAP_EXTENSION = ".map";

	public enum TILEMAP_ORIGIN // Origin is always at 0,0
	{
		TILEMAP_TOP_LEFT, // Starts at 0,0 and expands left down
		TILEMAP_CENTER, // Starts -x +y and expands left down
		TILEMAP_BOTTOM_LEFT, // Starts -x -y and expands left up
	};

	public enum TILE_ORIGIN
	{
		TILE_TOP_LEFT,
		TILE_CENTER,
		TILE_BOTTOM_LEFT,  
	};

    [Tooltip("Tile Set.")]
    public GameObject TileSet;

    //[Tooltip("Tiles blueprint for instantiating.")]
    protected GameObject[] tileBlueprints = new GameObject[(int)Tile.TILE_TYPE.NUM_TILE];
	[Tooltip("Default tile if no tile is specified.")]
	public Tile.TILE_TYPE DefaultTile = Tile.TILE_TYPE.TILE_EMPTY;

	// Tile map data
	[Tooltip("Origin point of Tile Map.")]
    protected TILEMAP_ORIGIN TileMapOrigin = TILEMAP_ORIGIN.TILEMAP_CENTER;
	[Tooltip("Origin point of Tile.")]
	protected TILE_ORIGIN TileOrigin = TILE_ORIGIN.TILE_CENTER;
	[Tooltip("Name of map file.")]
	public string Name = "";
	[Tooltip("Number of tile(s) vertically.")]
	public int NumOfTiles = 9;
    [Tooltip("The center point of the tile map.")]
    public Vector2 CenterPoint = new Vector2(0, 0);
    [Tooltip("The total size of the tile map.")]
    public Vector2 TotalSize = new Vector2(1, 1);

    public int TileSize { get { return tileSize; } }

    protected Vector2 NumOfScreenTiles = new Vector2();
    protected int tileSize = 32;
    protected Vector3 tileMapDistToTopLeft = new Vector3();
    protected int rowCount, colCount;

    // For tile map activation optimisation
    protected int minRowIndex = -1, maxRowIndex = -1, minColIndex = -1, maxColIndex = -1;


    protected List<Tile> tiles = new List<Tile>();

    // Map
    protected List<Row> map = new List<Row>();

	// Use this for initialization
	void Awake ()
	{
        
    }

    // Use this for initialization
    protected virtual void Start()
    {
        if (!TileSet)
        {
            throw new UnityException("No tile set given");
        }

        // Assign tiles blueprint from tile set
        Tile[] blueprints = TileSet.GetComponentsInChildren<Tile>(true);
        for (int index = 0; index < blueprints.Count(); ++index)
        {
            tileBlueprints[index] = blueprints[index].gameObject;
        }

        /*if (Name != "")
        {
            loadFile();
        }*/
    }
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    public void Load(string name, int numOfTiles = 18)
    {
        Name = name;
        NumOfTiles = numOfTiles;
        loadFile();
    }

	public MultiLayerTile FetchTile(Vector3 position)
	{
		Vector2 tileIndex = FetchTileIndex(position);
		return FetchTile((int)tileIndex.x, (int)tileIndex.y);
	}

	public MultiLayerTile FetchTile(int rowIndex, int colIndex)
	{
        if (rowIndex < 0 || rowIndex >= map.Count || colIndex < 0 || colIndex >= map[0].column.Count)
        {
            return null;
        }
		Row row = map[rowIndex];
		MultiLayerTile tiles = row.column[colIndex];
		return tiles;
	}

	public Vector2 FetchTileIndex(Vector3 position)
	{
		Vector3 posFromTopRight = position + tileMapDistToTopLeft;
		posFromTopRight.y = -(posFromTopRight.y);

		int rowIndex = (int)(posFromTopRight.y / tileSize);
		int colIndex = (int)(posFromTopRight.x / tileSize);

        if (rowIndex < 0)
        {
            rowIndex = 0;
        }

		return new Vector2(rowIndex, colIndex);
	}

    public void ActivateAllTiles(bool mode)
    {
        ActivateTiles(0, rowCount - 1, 0, colCount - 1, mode);
    }

	public void ActivateTiles(Vector3 topLeftPos, Vector3 bottomRightPos)
	{
		Vector2 topLeftIndex = FetchTileIndex(topLeftPos);
		Vector2 bottomRightIndex = FetchTileIndex(bottomRightPos);
		ActivateTiles((int)topLeftIndex.x, (int)bottomRightIndex.x, (int)topLeftIndex.y, (int)bottomRightIndex.y);
    }

    private void ActivateTiles(int rowIndexMin, int rowIndexMax, int colIndexMin, int colIndexMax, bool mode = true)
    {
        //if (!isActivated()) // Check if first update is required
        {
            // First tile update is required (Full activation)
            IEnumerable<Tile> activeTiles = from tile in tiles where tile.gameObject.activeSelf select tile;
            foreach (Tile singleTile in activeTiles)
            {
                singleTile.gameObject.SetActive(false);
            }

            for (int rowIndex = rowIndexMin; rowIndex <= rowIndexMax; ++rowIndex)
            {
                for (int colIndex = colIndexMin; colIndex <= colIndexMax; ++colIndex)
                {
                    MultiLayerTile multiTiles = FetchTile(rowIndex, colIndex);
                    if (multiTiles != null)
                    {
                        for (int multiIndex = 0; multiIndex < multiTiles.multiLayerTile.Count; ++multiIndex)
                        {
                            GameObject goTile = multiTiles.multiLayerTile[multiIndex];
                            goTile.SetActive(mode);
                        }
                    }
                }
            }
            minRowIndex = rowIndexMin;
            maxRowIndex = rowIndexMax;
            minColIndex = colIndexMin;
            maxColIndex = colIndexMax;
        }
        /*else
        {
            runtimeActivateTiles(rowIndexMin, rowIndexMax, colIndexMin, colIndexMax);
        }*/
    }

    private void runtimeActivateTiles(int rowIndexMin, int rowIndexMax, int colIndexMin, int colIndexMax)
    {
        int xDiff = colIndexMin - minColIndex; // New - Old
        int yDiff = rowIndexMin - minRowIndex; // New - Old

        if (xDiff == 0 || yDiff == 0)
        {
            // No update needed
            return;
        }

        int startRow, endRow, startCol, endCol;     // Activation
        int dStartRow, dEndRow, dStartCol, dEndCol; // De-activation

        // Note: min and max in front = Old
        // Note: min and max behind = New

        // Updating x-axis
        if (xDiff < 0)
        {
            // Move left
            startCol = colIndexMin;
            endCol = minColIndex;
            dStartCol = colIndexMax + 1;
            dEndCol = maxColIndex + 1;

            startRow = minRowIndex;
            endRow = maxRowIndex;
            ActivateTilesByIndex(startRow, endRow, startCol, endCol, true);
            ActivateTilesByIndex(startRow, endRow, dStartCol, dEndCol, false);

        }
        else if (xDiff > 0)
        {
            // Move right
            startCol = maxColIndex + 1;
            endCol = colIndexMax + 1;
            dStartCol = minColIndex;
            dEndCol = colIndexMin;

            startRow = minRowIndex;
            endRow = maxRowIndex;
            ActivateTilesByIndex(startRow, endRow, startCol, endCol, true);
            ActivateTilesByIndex(startRow, endRow, dStartCol, dEndCol, false);
        }
        minColIndex = colIndexMin;
        maxColIndex = colIndexMax;

        // Updating y-axis
        if (yDiff < 0)
        {
            // Move up
            startRow = rowIndexMin;
            endRow = minRowIndex;
            dStartRow = rowIndexMax + 1;
            dEndRow = maxRowIndex + 1;

            startCol = minColIndex;
            endCol = maxColIndex;
            ActivateTilesByIndex(startRow, endRow, startCol, endCol, true);
            ActivateTilesByIndex(dStartRow, dEndRow, startCol, endCol, false);
        }
        else if (yDiff > 0)
        {
            // Move down
            startRow = maxRowIndex + 1;
            endRow = rowIndexMax + 1;
            dStartRow = minRowIndex;
            dEndRow = rowIndexMin;

            startCol = minColIndex;
            endCol = maxColIndex;
            ActivateTilesByIndex(startRow, endRow, startCol, endCol, true);
            ActivateTilesByIndex(dStartRow, dEndRow, startCol, endCol, false);
        }
        minRowIndex = rowIndexMin;
        maxRowIndex = rowIndexMax;
    }

    private void ActivateTilesByIndex(int startRow, int endRow, int startCol, int endCol, bool activation)
    {
        for (int rowIndex = startRow; rowIndex < endRow; ++rowIndex)
        {
            for (int colIndex = startCol; colIndex < endCol; ++colIndex)
            {
                MultiLayerTile mTile = FetchTile(rowIndex, colIndex);
                if (mTile != null)
                {
                    for (int tileIndex = 0; tileIndex < mTile.multiLayerTile.Count; ++tileIndex)
                    {
                        GameObject tile = mTile.multiLayerTile[tileIndex];
                        tile.SetActive(activation);
                    }
                }
            }
        }
    }

	private bool loadFile()
	{
		int numRow = 0, numCol= 0;
		ArrayList sMap = new ArrayList(); // Write data into
        if (!File.Exists(MAP_DIRECTORY + Name + MAP_EXTENSION))
        {
            return false;
        }
        StreamReader file = new StreamReader(File.OpenRead(MAP_DIRECTORY + Name + MAP_EXTENSION)); // Open file
		while (!file.EndOfStream)
		{
			string line = file.ReadLine();
			if (line.StartsWith("//"))
			{
				continue; // Ignores commented line
			}
			string[] tokens = line.Split(',');
			int newLength = tokens.Length;
			if (newLength > numCol)
			{
				numCol = newLength;
			}
			sMap.Add(line);
		}
		numRow = sMap.Count;
        file.Close();
		if (generateMap(sMap, numRow, numCol))
		{
			return true;
		}
		return false;
	}

	private bool generateMap(ArrayList sMap, int numRow, int numCol)
	{
		tileSize = calculateTileSize(TotalSize);
		tileMapDistToTopLeft = generateDistToTopLeft(numRow, numCol);
		rowCount = numRow;
		colCount = numCol;

		// Calculate data needed
		Vector3 size = new Vector3(tileSize, tileSize, 1); // Size of tile (Scale)
		Vector3 startPos = generateStartPos(numRow, numCol); // Calculate start position

		// Generate map
		for (int rowIndex = 0; rowIndex < numRow;) // Loop for rows
		{
			// Data
			string line = (string)sMap[rowIndex];
			string[] tokens = line.Split(TILE_SPLIT); // Split each tile with ','

			//ArrayList rowOfData = new ArrayList(); // One row of tile
			Row rowOfData = new Row();

			for (int colIndex = 0; colIndex < numCol;) // Loop for cols
			{
				GameObject tile;
				if (colIndex < tokens.Length) // Use data from file
				{
					// Split different layer tiles
					string combinedLayerTiles = tokens[colIndex];
					string[] layers = combinedLayerTiles.Split(TILE_MULTIPLE_LAYER_SPLIT);

					//ArrayList multiLayerTile = new ArrayList();
					MultiLayerTile multiLayerTile = new MultiLayerTile();

					for (int layerIndex = 0; layerIndex < layers.Length; ++layerIndex)
					{
						int tileType = Int32.Parse(layers[layerIndex]);
						tile = createTile((Tile.TILE_TYPE)tileType, startPos, size);

                        if (tile)
                        {
                            // Add to multi-layer
                            multiLayerTile.AddTile(tile);

                            // Add to tile list
                            tiles.Add(tile.GetComponent<Tile>());
                        }
					}

					// Add to row of data
					rowOfData.column.Add(multiLayerTile);
				}
				else // Data not within file, empty tile
				{
					MultiLayerTile multiLayerTile = new MultiLayerTile();
					tile = createTile(DefaultTile, startPos, size);

                    if (tile)
                    {
                        // Add to multi-layer
                        multiLayerTile.AddTile(tile);

                        // Add to tile list
                        tiles.Add(tile.GetComponent<Tile>());
                    }

					// Add to row of data
					rowOfData.column.Add(multiLayerTile);
				}

				// Next col startPos
				startPos = generateStartPos(numRow, numCol, rowIndex, ++colIndex);
			}

			// Add row of data into map
			map.Add(rowOfData);

			// Next row startPos
			startPos = generateStartPos(numRow, numCol, ++rowIndex, 0);
		}
		return true;
	}

	private Vector3 generateStartPos(int numRow, int numCol, int rowIndex = 0, int colIndex = 0)
	{
		Vector3 startPos = CenterPoint;
		startPos += new Vector3(tileSize * colIndex, -tileSize * rowIndex, 2.0f);

		switch (TileMapOrigin)
		{
			case TILEMAP_ORIGIN.TILEMAP_TOP_LEFT:
				{
					switch (TileOrigin)
					{
						case TILE_ORIGIN.TILE_TOP_LEFT:
							{
								// Do nothing
							}
							break;
						case TILE_ORIGIN.TILE_CENTER:
							{
								startPos += new Vector3(tileSize * 0.5f, -tileSize * 0.5f);
							}
							break;
						case TILE_ORIGIN.TILE_BOTTOM_LEFT:
							{
								startPos += new Vector3(0, -tileSize);
							}
							break;
					}
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_CENTER:
				{
					float halfWidth = numCol * tileSize * 0.5f;
					float halfHeight = numRow * tileSize * 0.5f;
					startPos += new Vector3(-halfWidth, halfHeight, 0);
					switch (TileOrigin)
					{
						case TILE_ORIGIN.TILE_TOP_LEFT:
							{
								// Do nothing
							}
							break;
						case TILE_ORIGIN.TILE_CENTER:
							{
								startPos += new Vector3(tileSize * 0.5f, -tileSize * 0.5f);
							}
							break;
						case TILE_ORIGIN.TILE_BOTTOM_LEFT:
							{
								startPos += new Vector3(0, -tileSize);
							}
							break;
					}
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_BOTTOM_LEFT:
				{
					switch (TileOrigin)
					{
						case TILE_ORIGIN.TILE_TOP_LEFT:
							{
								startPos += new Vector3(0, tileSize);
							}
							break;
						case TILE_ORIGIN.TILE_CENTER:
							{
								startPos += new Vector3(tileSize * 0.5f, tileSize * 0.5f);
							}
							break;
						case TILE_ORIGIN.TILE_BOTTOM_LEFT:
							{
								// Do nothing
							}
							break;
					}
				}
				break;
		}

		return startPos;
	}

	private Vector3 generateDistToTopLeft(int numRow, int numCol)
	{
        Vector3 topLeft = Vector3.zero;//CenterPoint;
		switch (TileMapOrigin)
		{
			case TILEMAP_ORIGIN.TILEMAP_TOP_LEFT:
				{
					// Do nothing
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_CENTER:
				{
					topLeft += new Vector3(numCol * tileSize * 0.5f, -(numRow * tileSize * 0.5f), 0.0f);
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_BOTTOM_LEFT:
				{
					topLeft += new Vector3(0.0f, -(numRow * tileSize), 0.0f);
				}
				break;
		}
		return topLeft - new Vector3(CenterPoint.x, CenterPoint.y);
	}

	protected int calculateTileSize(Vector2 screenSize)
	{
		NumOfScreenTiles.y = NumOfTiles;

		tileSize = (int)Math.Ceiling(screenSize.y / NumOfScreenTiles.y);
		NumOfScreenTiles.x = (int)Math.Ceiling(screenSize.x / tileSize);

		return tileSize;
	}

	protected virtual GameObject createTile(Tile.TILE_TYPE type, Vector3 pos, Vector3 size)
	{
        if (type == Tile.TILE_TYPE.TILE_EMPTY)
        {
            return null;
        }
        if (!tileBlueprints[(int)type] && type != Tile.TILE_TYPE.TILE_FIRST_PLAYER && type !=  Tile.TILE_TYPE.TILE_SECOND_PLAYER && type != Tile.TILE_TYPE.TILE_ENEMY && type != Tile.TILE_TYPE.TILE_WAYPOINT)
        {
            return null;
        }

        GameObject tile = null;

		switch (type)
		{
            default:
                {
                    tile = Instantiate(tileBlueprints[(int)type]);

                    // Set data for each tile
                    tile.SetActive(false);
                    tile.transform.position = pos;
                    tile.transform.localScale = size;
                    tile.transform.parent = this.transform;
                }
                break;
		}
        return tile;
	}

    private bool isActivated()
    {
        return !(minRowIndex == -1 || maxRowIndex == -1 || minColIndex == -1 || maxColIndex == -1);
        
    }
}
