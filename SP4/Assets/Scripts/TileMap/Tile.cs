using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public enum TILE_TYPE
	{
		TILE_EMPTY = 0, // Empty tile
		TILE_FLOOR,     // Floor tile
		TILE_LAYER,     // Layer tile test
        TILE_ENEMY,     // Tile that spawns an enemy

		NUM_TILE,       // Total number of tiles
	};

	public TILE_TYPE Type = TILE_TYPE.TILE_EMPTY;

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
