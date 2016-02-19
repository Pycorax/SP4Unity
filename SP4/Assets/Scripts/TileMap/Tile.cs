using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public enum TILE_TYPE
	{
		TILE_EMPTY = 0, // Empty tile
		TILE_FLOOR,     // Floor tile
		TILE_LAYER,     // Layer tile test

        // Entities (Tiles not on tile sheet)
        TILE_ENEMY,     // Tile that spawns an enemy
        TILE_WAYPOINT,  // Enemy's waypoint
        TILE_FIRST_PLAYER, // First player
        TILE_SECOND_PLAYER, // Second player

		NUM_TILE,       // Total number of tiles
	};

    [Tooltip("Type of tile.")]
    public TILE_TYPE Type = TILE_TYPE.TILE_EMPTY;
    [Tooltip("Scale ratio according to tile size from Tile Map.")]
    public float ScaleRatio = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsWalkable()
	{
		if (!GetComponent<Collider2D>())
		{
			if (!IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsEmpty()
	{
		return Type == TILE_TYPE.TILE_EMPTY;
	}
}
