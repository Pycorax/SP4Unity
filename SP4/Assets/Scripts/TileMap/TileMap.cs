using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/**********
Multi-layer tile system
NOTE: Writing format is from top to bottom. No spaces allowed
Example: Tile A is supposed to be rendered above Tile B, writing format is "A|B"
**********/

public class MultiLayerTile
{
	public List<GameObject> multiLayerTile = new List<GameObject>();
}

public class Row
{
	public List<MultiLayerTile> column = new List<MultiLayerTile>();
}

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
	
	[Tooltip("Tiles blueprint for instantiating.")]
	public GameObject[] TileBlueprints = new GameObject[(int)Tile.TILE_TYPE.NUM_TILE];
	[Tooltip("Default tile if no tile is specified.")]
	public Tile.TILE_TYPE DefaultTile = Tile.TILE_TYPE.TILE_EMPTY;

	// Tile map data
	[Tooltip("Origin point of Tile Map.")]
	public TILEMAP_ORIGIN TileMapOrigin = TILEMAP_ORIGIN.TILEMAP_CENTER;
	[Tooltip("Origin point of Tile.")]
	public TILE_ORIGIN TileOrigin = TILE_ORIGIN.TILE_CENTER;
	[Tooltip("Path to map file.")]
	public string Filepath = "";
	[Tooltip("Number of tile(s) vertically.")]
	public int NumOfTiles = 9;

	private Vector2 NumOfScreenTiles = new Vector2();
	private int TileSize = 32;
	private Vector3 tileMapDistToTopLeft = new Vector3();
	private int rowCount, colCount;


	private List<Tile> tiles = new List<Tile>();

	// Map
	//private ArrayList map = new ArrayList();
	private List<Row> map = new List<Row>();


	// Use this for initialization
	void Start ()
	{
		if (Filepath != "")
		{
			loadFile();
		}
		//ActivateTiles(0, rowCount - 1, 0, colCount - 1);
		//ActivateTiles(1, 13, 5, 26);
		ActivateTiles(map[1].column[5].multiLayerTile[0].transform.position - new Vector3(TileSize * 0.5f, 0.0f), map[13].column[26].multiLayerTile[0].transform.position - new Vector3(TileSize * 0.5f, 0.0f));
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public MultiLayerTile FetchTile(Vector3 position)
	{
		Vector2 tileIndex = FetchTileIndex(position);
		return FetchTile((int)tileIndex.x, (int)tileIndex.y);
	}

	public MultiLayerTile FetchTile(int rowIndex, int colIndex)
	{
		List<GameObject> result = new List<GameObject>();

		//ArrayList row = (ArrayList)map[rowIndex];
		//ArrayList multiLayerTile = (ArrayList)row[colIndex];

		Row row = map[rowIndex];
		MultiLayerTile tiles = row.column[colIndex];
		return tiles;
	}

	public Vector2 FetchTileIndex(Vector3 position)
	{
		Vector3 posFromTopRight = position + tileMapDistToTopLeft;
		posFromTopRight.y = Math.Abs(posFromTopRight.y);

		int rowIndex = (int)(posFromTopRight.y / TileSize);//(rowCount * TileSize / posFromTopRight.y);
		int colIndex = (int)(posFromTopRight.x / TileSize);//(colCount * TileSize / posFromTopRight.x);

		return new Vector2(rowIndex, colIndex);
	}

	public void ActivateTiles(int rowIndexMin, int rowIndexMax, int colIndexMin, int colIndexMax)
	{
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
				for (int multiIndex = 0; multiIndex < multiTiles.multiLayerTile.Count; ++multiIndex)
				{
					GameObject goTile = multiTiles.multiLayerTile[multiIndex];
					goTile.SetActive(true);
				}
			}
		}
	}

	public void ActivateTiles(Vector3 topLeftPos, Vector3 bottomRightPos)
	{
		Vector2 topLeftIndex = FetchTileIndex(topLeftPos);
		Vector2 bottomRightIndex = FetchTileIndex(bottomRightPos);
		ActivateTiles((int)topLeftIndex.x, (int)bottomRightIndex.x, (int)topLeftIndex.y, (int)bottomRightIndex.y);
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
		tileMapDistToTopLeft = generateDistToTopLeft(numRow, numCol);
		rowCount = numRow;
		colCount = numCol;

		// Calculate data needed
		Vector3 size = new Vector3(TileSize, TileSize, 1); // Size of tile (Scale)
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
						tile = Instantiate(TileBlueprints[tileType]);
						
						// Set common data for each tile
						tile.SetActive(false);
						tile.transform.position = startPos; // CHECK: Copy by value or reference
						tile.transform.localScale = size; // CHECK: Copy by value or reference
						tile.transform.parent = this.transform;

						// Add to multi-layer
						multiLayerTile.multiLayerTile.Add(tile);

						// Add to tile list
						tiles.Add(tile.GetComponent<Tile>());
					}

					// Add to row of data
					rowOfData.column.Add(multiLayerTile);
				}
				else // Data not within file, empty tile
				{
					MultiLayerTile multiLayerTile = new MultiLayerTile();
					tile = Instantiate(TileBlueprints[(int)DefaultTile]);

					// Set common data for each tile
					tile.SetActive(false);
					tile.transform.position = startPos; // CHECK: Copy by value or reference
					tile.transform.localScale = size; // CHECK: Copy by value or reference
					tile.transform.parent = this.transform;

					// Add to multi-layer
					multiLayerTile.multiLayerTile.Add(tile);

					// Add to tile list
					tiles.Add(tile.GetComponent<Tile>());

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

	private Vector3 generateDistToTopLeft(int numRow, int numCol)
	{
		Vector3 topLeft = Vector3.zero;
		switch (TileMapOrigin)
		{
			case TILEMAP_ORIGIN.TILEMAP_TOP_LEFT:
				{
					// Do nothing
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_CENTER:
				{
					topLeft.Set(numCol * TileSize * 0.5f, -(numRow * TileSize * 0.5f), 0.0f);
				}
				break;
			case TILEMAP_ORIGIN.TILEMAP_BOTTOM_LEFT:
				{
					topLeft.Set(0.0f, -(numRow * TileSize), 0.0f);
				}
				break;
		}
		return topLeft;
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
