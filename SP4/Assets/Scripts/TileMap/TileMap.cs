using UnityEngine;
using System;
using System.Collections;
using System.IO;

/**********
Multi-layer tile system
NOTE: Writing format is from top to bottom. No spaces allowed
Example: Tile A is supposed to be rendered above Tile B, writing format is "A|B"
**********/

public class TileMap : MonoBehaviour
{
    public static char TILE_SPLIT = ',';
    public static char TILE_MULTIPLE_LAYER_SPLIT = '|';

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

	// Tiles blueprint for instantiating
	public GameObject[] TileBlueprints = new GameObject[(int)Tile.TILE_TYPE.NUM_TILE];
	Tile.TILE_TYPE DefaultTile = Tile.TILE_TYPE.TILE_EMPTY;

	// Tile map data
	public TILEMAP_ORIGIN TileMapOrigin = TILEMAP_ORIGIN.TILEMAP_CENTER;
	public TILE_ORIGIN TileOrigin = TILE_ORIGIN.TILE_CENTER;
	public string Filepath = "";
	public int NumOfTiles = 9;
	private Vector2 NumOfScreenTiles = new Vector2();
	private int TileSize = 32;

	// Map
	private ArrayList map = new ArrayList();


	// Use this for initialization
	void Start ()
	{
		if (Filepath != "")
		{
			loadFile();
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	private bool loadFile()
	{
		int numRow = 0, numCol= 0;
		ArrayList sMap = new ArrayList(); // Write data into
		StreamReader file = new StreamReader(File.OpenRead(Filepath)); // Open file
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
		if (generateMap(sMap, numRow, numCol))
		{
			return true;
		}
		return false;
	}

	private bool generateMap(ArrayList sMap, int numRow, int numCol)
	{
		TileSize = calculateTileSize();

		// Calculate data needed
		Vector3 size = new Vector3(TileSize, TileSize, 1); // Size of tile (Scale)
		Vector3 startPos = generateStartPos(numRow, numCol); // Calculate start position

		// Generate map
		for (int rowIndex = 0; rowIndex < numRow;) // Loop for rows
		{
			// Data
			string line = (string)sMap[rowIndex];
			string[] tokens = line.Split(TILE_SPLIT); // Split each tile with ','

			ArrayList rowOfData = new ArrayList(); // One row of tile

			for (int colIndex = 0; colIndex < numCol;) // Loop for cols
			{
				GameObject tile;
				if (colIndex < tokens.Length) // Use data from file
				{
                    // Split different layer tiles
                    string combinedLayerTiles = tokens[colIndex];
                    string[] layers = combinedLayerTiles.Split(TILE_MULTIPLE_LAYER_SPLIT);
                    
                    ArrayList multiLayerTile = new ArrayList();

                    for (int layerIndex = 0; layerIndex < layers.Length; ++layerIndex)
                    {
                        int tileType = Int32.Parse(layers[layerIndex]);
                        tile = Instantiate(TileBlueprints[tileType]);
                        
                        // Set common data for each tile
                        tile.SetActive(true);
                        tile.transform.position = startPos; // CHECK: Copy by value or reference
                        tile.transform.localScale = size; // CHECK: Copy by value or reference

                        // Add to multi-layer
                        multiLayerTile.Add(tile);
                    }

                    // Add to row of data
                    rowOfData.Add(multiLayerTile);
                }
				else // Data not within file, empty tile
                {
                    ArrayList multiLayerTile = new ArrayList();
                    tile = Instantiate(TileBlueprints[(int)DefaultTile]);

                    // Set common data for each tile
                    tile.SetActive(true);
                    tile.transform.position = startPos; // CHECK: Copy by value or reference
                    tile.transform.localScale = size; // CHECK: Copy by value or reference

                    multiLayerTile.Add(tile);
                    rowOfData.Add(multiLayerTile);
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
		Vector3 startPos = Vector3.zero;
		startPos += new Vector3(TileSize * colIndex, -TileSize * rowIndex);

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
								startPos += new Vector3(TileSize * 0.5f, -TileSize * 0.5f);
							}
							break;
						case TILE_ORIGIN.TILE_BOTTOM_LEFT:
							{
								startPos += new Vector3(0, -TileSize);
							}
							break;
					}
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_CENTER:
				{
					float halfWidth = numCol * TileSize * 0.5f;
					float halfHeight = numRow * TileSize * 0.5f;
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
								startPos += new Vector3(TileSize * 0.5f, -TileSize * 0.5f);
							}
							break;
						case TILE_ORIGIN.TILE_BOTTOM_LEFT:
							{
								startPos += new Vector3(0, -TileSize);
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
								startPos += new Vector3(0, TileSize);
							}
							break;
						case TILE_ORIGIN.TILE_CENTER:
							{
								startPos += new Vector3(TileSize * 0.5f, TileSize * 0.5f);
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

	private int calculateTileSize()
	{
		Vector2 screenSize = ScreenData.GetScreenSize();
		NumOfScreenTiles.y = NumOfTiles;

		TileSize = (int)Math.Ceiling(screenSize.y / NumOfScreenTiles.y);
		NumOfScreenTiles.x = (int)Math.Ceiling(screenSize.x / TileSize);

		return TileSize;
	}
}
