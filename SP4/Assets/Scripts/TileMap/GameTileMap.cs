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
    }

    public void Load()
    {
        Load(Name, NumOfTiles);
        // Sync waypoints
        WaypointManager refWaypointManager = this.transform.root.gameObject.GetComponentInChildren<WaypointManager>();
        refWaypointManager.SyncWaypoints();
    }

    protected override GameObject createTile(Tile.TILE_TYPE type, Vector3 pos, Vector3 size)
    {
        if (type == Tile.TILE_TYPE.TILE_EMPTY || type >= Tile.TILE_TYPE.NUM_TILE)
        {
            return null;
        }
        if (!tileBlueprints[(int)type])
        {
            return null;
        }

        GameObject tile = null;
        float scaleRatio = tileBlueprints[(int)type].GetComponent<Tile>().ScaleRatio;

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
                        Vector3 enemyPos = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                        enemyPos.z = 1.0f;
                        Vector3 enemySize = size * scaleRatio;
                        enemy.SetActive(true);
                        enemy.GetComponent<Enemy.Enemy>().Init(enemyPos);
                        enemy.transform.localScale = enemySize;
                    }
                }
                break;
            case Tile.TILE_TYPE.TILE_WAYPOINT:
                {
                    WaypointManager refWaypointManager = this.transform.root.gameObject.GetComponentInChildren<WaypointManager>();
                    if (refWaypointManager)
                    {
                        // Create waypoint
                        GameObject waypoint = Instantiate(tileBlueprints[(int)type]);
                        Vector3 waypointPos = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                        waypointPos.z = 1.5f;
                        Vector3 waypointSize = size * scaleRatio;
                        waypoint.transform.position = waypointPos;
                        waypoint.transform.localScale = waypointSize;
                        refWaypointManager.Add(waypoint.GetComponent<Waypoint>());
                    }
                }
                break;
            case Tile.TILE_TYPE.TILE_FIRST_PLAYER:
                {
                    Vector3 playerPos = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                    Vector3 playerSize = size * scaleRatio;
                    playerPos.z = 0.0f;
                    RefPlayer1.transform.position = playerPos;
                    RefPlayer1.transform.localScale = playerSize;
                }
                break;
            case Tile.TILE_TYPE.TILE_SECOND_PLAYER:
                {
                    Vector3 playerPos = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                    Vector3 playerSize = size * scaleRatio;
                    playerPos.z = 0.0f;
                    RefPlayer2.transform.position = playerPos;
                    RefPlayer2.transform.localScale = playerSize;
                }
                break;
            default:
                {
                    tile = Instantiate(tileBlueprints[(int)type]);

                    if (tile.GetComponent<Item>() != null || type == Tile.TILE_TYPE.TILE_SPIKE_TRAP || type == Tile.TILE_TYPE.TILE_CANNON)
                    {
                        pos.z -= 1;
                        tile.SetActive(true);
                    }
                    else
                    {
                        tile.SetActive(false);
                    }

                    // Set data for each tile
                    tile.transform.position = pos + new Vector3((scaleRatio - 1) * tileSize * 0.5f, -((scaleRatio - 1) * tileSize * 0.5f));
                    tile.transform.localScale = size * scaleRatio;
                    tile.transform.parent = this.transform;
                }
                break;
        }
        return tile;
    }
}
