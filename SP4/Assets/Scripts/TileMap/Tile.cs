using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	public enum TILE_TYPE
    {
        TILE_EMPTY = 0, // Empty tile

        // Chair 1
        TILE_CHAIR_1_RIGHT,
        TILE_CHAIR_1_UP,
        TILE_CHAIR_1_LEFT,
        TILE_CHAIR_1_DOWN,

        // Barricade
        TILE_BARRICADE_VERTICAL,
        TILE_BARRICADE_HORIZONTAL,

        // Barrel
        TILE_BARREL,

        // Wall 1
        TILE_WALL_1_CORNER_TOP_LEFT,
        TILE_WALL_1_HORIZONTAL_1,
        TILE_WALL_1_HORIZONTAL_2,
        TILE_WALL_1_HORIZONTAL_3,
        TILE_WALL_1_CORNER_TOP_RIGHT,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_1_1,
        TILE_CARPET_1_1_2,
        TILE_CARPET_1_1_3,
        TILE_CARPET_1_1_4,

        // Wall 2
        TILE_WALL_2_CORNER,
        TILE_WALL_2_HORIZONTAL_1,
        TILE_WALL_2_HORIZONTAL_2,
        TILE_WALL_2_HORIZONTAL_3,

        // Chair 2
        TILE_CHAIR_2_UP,
        TILE_CHAIR_2_LEFT,
        TILE_CHAIR_2_RIGHT,
        TILE_CHAIR_2_DOWN,

        // Wall 1
        TILE_WALL_1_VERTICAL_1,

        // Table
        TILE_TABLE_1_1,
        TILE_TABLE_1_2,

        // Statue 1 [Last 2 number is ROW_COL]
        TILE_STATUE_1_1_1,
        TILE_STATUE_1_1_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_2_1,
        TILE_CARPET_1_2_2,
        TILE_CARPET_1_2_3,
        TILE_CARPET_1_2_4,

        // Wall 2
        TILE_WALL_2_VERTICAL_1,

        // Floor
        TILE_FLOOR_1,
        TILE_FLOOR_2,
        TILE_FLOOR_3,
        TILE_FLOOR_4,

        // Grass patch (Multi-tile structure)
        TILE_GRASS_PATCH_CORNER_TOP_LEFT,
        TILE_GRASS_PATCH_CORNER_TOP_MIDDLE,
        TILE_GRASS_PATCH_CORNER_TOP_RIGHT,

        // Wall 1
        TILE_WALL_1_VERTICAL_2,

        // Table
        TILE_TABLE_2_1,
        TILE_TABLE_2_2,

        // Statue 1 [Last 2 number is ROW_COL]
        TILE_STATUE_1_2_1,
        TILE_STATUE_1_2_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_3_1,
        TILE_CARPET_1_3_2,
        TILE_CARPET_1_3_3,
        TILE_CARPET_1_3_4,

        // Wall 2
        TILE_WALL_2_VERTICAL_2,

        // Grass block
        TILE_GRASS_1,
        TILE_GRASS_2,
        TILE_GRASS_3,

        // Bordered grass block
        TILE_BORDERED_GRASS_1,

        // Grass patch (Multi-tile structure)
        TILE_GRASS_PATCH_CORNER_LEFT,
        TILE_GRASS_PATCH,
        TILE_GRASS_PATCH_CORNER_RIGHT,

        // Wall 1
        TILE_WALL_1_VERTICAL_3,

        // Table
        TILE_TABLE_3_1,
        TILE_TABLE_3_2,

        // Statue 2 [Last 2 number is ROW_COL]
        TILE_STATUE_2_1_1,
        TILE_STATUE_2_1_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_4_1,
        TILE_CARPET_1_4_2,
        TILE_CARPET_1_4_3,
        TILE_CARPET_1_4_4,

        // Wall 2
        TILE_WALL_2_VERTICAL_3,

        // Brick floor
        TILE_BRICK_FLOOR_1,
        TILE_BRICK_FLOOR_2,
        TILE_BRICK_FLOOR_3,

        // Bordered grass block
        TILE_BORDERED_GRASS_2,

        // Grass patch (Multi-tile structure)
        TILE_GRASS_PATCH_CORNER_BOTTOM_LEFT,
        TILE_GRASS_PATCH_CORNER_BOTTOM,
        TILE_GRASS_PATCH_CORNER_BOTTOM_RIGHT,

        // Wall 1
        TILE_WALL_1_CORNER_BOTTOM_LEFT,
        
        // Table
        TILE_TABLE_4_1,
        TILE_TABLE_4_2,

        // Statue 2 [Last 2 number is ROW_COL]
        TILE_STATUE_2_2_1,
        TILE_STATUE_2_2_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_5_1,
        TILE_CARPET_1_5_2,
        TILE_CARPET_1_5_3,
        TILE_CARPET_1_5_4,

        // Carpet 2 (Purple carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_2_1_1,
        TILE_CARPET_2_1_2,
        TILE_CARPET_2_1_3,
        TILE_CARPET_2_1_4,

        // Carpet 3 (Brown carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_3_1_1,
        TILE_CARPET_3_1_2,
        TILE_CARPET_3_1_3,
        TILE_CARPET_3_1_4,

        // Wall 1
        TILE_WALL_1_CORNER_BOTTOM_RIGHT,

        // Window
        TILE_WINDOW_RIGHT,
        TILE_WINDOW_LEFT,

        // Statue 3 [Last 2 number is ROW_COL]
        TILE_STATUE_3_1_1,
        TILE_STATUE_3_1_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_6_1,
        TILE_CARPET_1_6_2,
        TILE_CARPET_1_6_3,
        TILE_CARPET_1_6_4,

        // Carpet 2 (Purple carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_2_2_1,
        TILE_CARPET_2_2_2,
        TILE_CARPET_2_2_3,
        TILE_CARPET_2_2_4,

        // Carpet 3 (Brown carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_3_2_1,
        TILE_CARPET_3_2_2,
        TILE_CARPET_3_2_3,
        TILE_CARPET_3_2_4,

        // Random
        TILE_RANDOM,

        // Window
        TILE_WINDOW_DOWN,
        TILE_WINDOW_UP,

        // Statue 3 [Last 2 number is ROW_COL]
        TILE_STATUE_3_2_1,
        TILE_STATUE_3_2_2,

        // Carpet 1 (Long vertical carpet) [Last 2 number is ROW_COL]
        TILE_CARPET_1_7_1,
        TILE_CARPET_1_7_2,
        TILE_CARPET_1_7_3,
        TILE_CARPET_1_7_4,


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
