using UnityEngine;
using System.Collections;

public class GameTileMap : TileMap
{
    [Tooltip("Player 1 reference.")]
    public GameObject RefPlayer1;

    [Tooltip("Player 2 reference.")]
    public GameObject RefPlayer2;

    [Tooltip("Enemy Manager reference.")]
    public ResourceManager RefEnemyManager;

    // Use this for initialization
    protected override void Start ()
    {
        TotalSize = ScreenData.GetScreenSize();
        tileSize = calculateTileSize(TotalSize);
        base.Start();
        Load(Name, NumOfTiles);
        // Sync waypoints
        WaypointManager refWaypointManager = this.transform.root.gameObject.GetComponentInChildren<WaypointManager>();
        refWaypointManager.SyncWaypoints();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    protected override GameObject createTile(Tile.TILE_TYPE type, Vector3 pos, Vector3 size)
    {
        if (type == Tile.TILE_TYPE.TILE_EMPTY)
        {
            return null;
        }
        if (!tileBlueprints[(int)type] && type != Tile.TILE_TYPE.TILE_FIRST_PLAYER && type != Tile.TILE_TYPE.TILE_SECOND_PLAYER && type != Tile.TILE_TYPE.TILE_ENEMY && type != Tile.TILE_TYPE.TILE_WAYPOINT)
        {
            return null;
        }

        GameObject tile = null;

        switch (type)
        {
            // TODO: Add special case for tile creation like enemy
            case Tile.TILE_TYPE.TILE_ENEMY:
                {
                    // Create enemy
                    GameObject enemy = RefEnemyManager.Fetch();
                    if (enemy)
                    {
                        // Set enemy data
                        Vector3 enemyPos = pos + (new Vector3(size.x, -size.y) * 0.5f);
                        enemyPos.z = 1.0f;
                        Vector3 enemySize = size * 2.0f;
                        enemy.SetActive(true);
                        enemy.GetComponent<Enemy.Enemy>().Init(enemyPos);
                        enemy.transform.localScale = enemySize;
                    }

                    /*GameObject enemy = Instantiate(TileBlueprints[(int)type]);
                    // Set enemy data
                    Vector3 enemyPos = pos + (new Vector3(size.x, -size.y) * 0.5f);
                    enemyPos.z = 1.0f;
                    Vector3 enemySize = size * 2.0f;
                    enemy.SetActive(true);
                    enemy.GetComponent<Enemy.Enemy>().Init(enemyPos);// = pos + new Vector3(size.x, -size.y);
                    enemy.transform.localScale = enemySize;
                    // Assign waypoint map to enemy
                    WaypointManager refWaypointManager = this.transform.root.gameObject.GetComponentInChildren<WaypointManager>();
                    enemy.GetComponent<Enemy.Enemy>().WaypointMap = refWaypointManager;
                    enemyList.Add(enemy);*/

                    /*// Create floor tile
                    tile = Instantiate(TileBlueprints[(int)Tile.TILE_TYPE.TILE_FLOOR_1]);
                    // Set data for each tile
                    tile.SetActive(false);
                    tile.transform.position = pos;
                    tile.transform.localScale = size;
                    tile.transform.parent = this.transform;*/
                }
                break;
            case Tile.TILE_TYPE.TILE_WAYPOINT:
                {
                    WaypointManager refWaypointManager = this.transform.root.gameObject.GetComponentInChildren<WaypointManager>();
                    if (refWaypointManager)
                    {
                        // Create waypoint
                        GameObject waypoint = Instantiate(tileBlueprints[(int)type]);
                        Vector3 waypointPos = pos + (new Vector3(size.x, -size.y) * 0.5f);
                        waypointPos.z = 1.5f;
                        Vector3 waypointSize = size * 2.0f;
                        waypoint.transform.position = waypointPos;
                        waypoint.transform.localScale = waypointSize;
                        refWaypointManager.Add(waypoint.GetComponent<Waypoint>());
                    }

                    /*// Create floor tile
                    tile = Instantiate(TileBlueprints[(int)Tile.TILE_TYPE.TILE_FLOOR_1]);
                    // Set data for each tile
                    tile.SetActive(false);
                    tile.transform.position = pos;
                    tile.transform.localScale = size;
                    tile.transform.parent = this.transform;*/
                }
                break;
            case Tile.TILE_TYPE.TILE_FIRST_PLAYER:
                {
                    Vector3 playerPos = pos + (new Vector3(size.x, -size.y) * 0.5f);
                    Vector3 playerSize = size * 2.0f;
                    playerPos.z = 0.0f;
                    RefPlayer1.transform.position = playerPos;
                    RefPlayer1.transform.localScale = playerSize;

                    // Create floor tile
                    /*tile = Instantiate(TileBlueprints[(int)Tile.TILE_TYPE.TILE_FLOOR_1]);
                    // Set data for each tile
                    tile.SetActive(false);
                    tile.transform.position = pos;
                    tile.transform.localScale = size;
                    tile.transform.parent = this.transform;*/
                }
                break;
            case Tile.TILE_TYPE.TILE_SECOND_PLAYER:
                {
                    Vector3 playerPos = pos + (new Vector3(size.x, -size.y) * 0.5f);
                    Vector3 playerSize = size * 2.0f;
                    playerPos.z = 0.0f;
                    RefPlayer2.transform.position = playerPos;
                    RefPlayer2.transform.localScale = playerSize;

                    // Create floor tile
                    /*tile = Instantiate(TileBlueprints[(int)Tile.TILE_TYPE.TILE_FLOOR_1]);
                    // Set data for each tile
                    tile.SetActive(false);
                    tile.transform.position = pos;
                    tile.transform.localScale = size;
                    tile.transform.parent = this.transform;*/
                }
                break;
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
}
