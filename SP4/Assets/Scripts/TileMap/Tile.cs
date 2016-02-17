using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public enum TILE_TYPE
	{
		TILE_EMPTY = 0, // Empty tile
		TILE_FLOOR,     // Floor tile

		NUM_TILE,       // Total number of tiles
	};

	public TILE_TYPE Type = TILE_TYPE.TILE_EMPTY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool IsCollidable()
	{
        if (!GetComponent<Collider2D>())
        {
            if (!IsEmpty())
            {
                return false;
            }
        }
        return true;
	}

    public bool IsEmpty()
    {
        return Type == TILE_TYPE.TILE_EMPTY;
    }
}
