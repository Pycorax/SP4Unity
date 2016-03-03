using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiPlayerCamera : MonoBehaviour
{
    enum BOUNDS_TYPE
    {
        BOUNDS_TOP = 0,
        BOUNDS_BOTTOM,
        BOUNDS_LEFT,
        BOUNDS_RIGHT,
        NUM_BOUNDS,
    }

    [Tooltip("List of GameObjects that the camera focus on.")]
    public List<GameObject> PlayerList;
    [Tooltip("Reference to the tile map.")]
    public TileMap TileMapReference;
    [Tooltip("Are we using a deadzone?")]
    public bool DeadZoneEnabled = true;
    [Tooltip("Left deadzone for the camera to start moving. This value is padding from the edge.")]
    public float LeftDeadZonePadding;
    [Tooltip("Right deadzone for the camera to start moving. This value is padding from the edge.")]
    public float RightDeadZonePadding;
    [Tooltip("Top deadzone for the camera to start moving. This value is padding from the edge.")]
    public float TopDeadZonePadding;
    [Tooltip("Bottom deadzone for the camera to start moving. This value is padding from the edge.")]
    public float BottomDeadZonePadding;
    [Tooltip("Time it takes for the camera to snap into place after the dead zone was exceeded.")]
    public float CameraSnapTime = 0.2f;
    [Tooltip("The speed of the camera to snap to the center point of the player.")]
    public float CameraSnapSpeed = 100.0f;
    [Tooltip("Bounds of the camera.")]
    public GameObject[] Bounds = new GameObject[(int)BOUNDS_TYPE.NUM_BOUNDS];


    // Static Constants
    private const int PLAYER_COUNT = 2;
    private const float DEADZONE_STOP_MOVEMENT_ACCURACY = 12.5f;

    // Dead Zone
    private bool moveTowardsCenterPoint = false;        // Set to true when it leaves the dead zone for the first time.

    // Components
    private new Camera camera;

    // Getters

    public float TopBound { get { return transform.position.y + ScreenData.GetScreenSize().y * 0.5f; } }
    public float BottomBound { get { return transform.position.y - ScreenData.GetScreenSize().y * 0.5f; } }
    public float LeftBound { get { return transform.position.x - ScreenData.GetScreenSize().x * 0.5f; } }
    public float RightBound { get { return transform.position.x + ScreenData.GetScreenSize().x * 0.5f; } }

    // Use this for initialization
    void Start ()
    {
        camera = GetComponent<Camera>();
        updateBounds();

        // Set position to the center of both players at the start
        // Get the half distance between the players
        Vector2 deltaPos = PlayerList[1].transform.position - PlayerList[0].transform.position;
        deltaPos *= 0.5f; // PLAYER_COUNT

        // Calculate the centerpoint
        Vector3 centerpoint = (Vector2)PlayerList[0].transform.position + deltaPos;
        centerpoint.z = -10;
        transform.position = centerpoint;
    }
	
    void OnDrawGizmos()
    {
        Vector2 topLeftBound = new Vector2(LeftBound + LeftDeadZonePadding, TopBound - TopDeadZonePadding);
        Vector2 botRightBound = new Vector2(RightBound - RightDeadZonePadding, BottomBound + BottomDeadZonePadding);

        Vector3 testObjPos = new Vector3(topLeftBound.x + (botRightBound.x - topLeftBound.x) * 0.5f, botRightBound.y + (topLeftBound.y - botRightBound.y) * 0.5f, 1.0f);

        // Draw the wire cube to represent the dead zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(testObjPos, new Vector3(botRightBound.x - topLeftBound.x, botRightBound.y - topLeftBound.y, 1));
    }

    // Update is called once per frame
    void Update ()
    {
        // Error checking. This script only works on 2 players.
        if (PlayerList.Count != PLAYER_COUNT)
        {
            throw new UnityException("Too many players assigned to MultiPlayerCamera!");
        }

        // Vector3 to store the center point
        Vector3 centerPoint;

        // Get a list of alive players
        var alivePlayers = PlayerList.Where(x => x.GetComponent<RPGPlayer>().IsAlive).ToList();

        // React accordingly to that
        if (alivePlayers.Count > 1)
        {
            // Get the half distance between the players
            Vector2 deltaPos = PlayerList[1].transform.position - PlayerList[0].transform.position;
            deltaPos *= 0.5f; // PLAYER_COUNT

            // Calculate the centerpoint
            centerPoint = (Vector2) PlayerList[0].transform.position + deltaPos;
        }
        else if (alivePlayers.Count > 0)
        {
            centerPoint = alivePlayers.First().transform.position;
        }
        else
        {
            // Game over, they're all dead
            return;
        }

        if (DeadZoneEnabled && !moveTowardsCenterPoint)
        {
            // Check if this point is outside the deadzone, if so, then we move
            Vector2 topLeftBound = new Vector2(LeftBound + LeftDeadZonePadding, TopBound - TopDeadZonePadding);
            Vector2 botRightBound = new Vector2(RightBound - RightDeadZonePadding, BottomBound + BottomDeadZonePadding);

            // Only move if we are out of the bounds
            if (!
                (
                    topLeftBound.x < centerPoint.x && botRightBound.x > centerPoint.x
                    &&
                    topLeftBound.y > centerPoint.y && botRightBound.y < centerPoint.y
                 )
                )
            {
                moveTowardsCenterPoint = true;
            }
        }
        
        if (!DeadZoneEnabled || moveTowardsCenterPoint)
        {
            // Get direction to the center point
            Vector2 posDelta = centerPoint - transform.position;

            // Get the distance we travel this frame
            Vector3 moveDelta = posDelta.normalized * CameraSnapSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            // Clamp the values
            if (Mathf.Abs(moveDelta.x) > Mathf.Abs(posDelta.x))
            {
                moveDelta.x = posDelta.x;
            }
            if (Mathf.Abs(moveDelta.y) > Mathf.Abs(posDelta.y))
            {
                moveDelta.y = posDelta.y;
            }

            // Get the distance we travel this frame and hence our new position
            Vector3 newPos = transform.position + moveDelta;
            transform.position = newPos;

            if (
                    moveTowardsCenterPoint
                    &&
                    (transform.position - centerPoint).sqrMagnitude < DEADZONE_STOP_MOVEMENT_ACCURACY * DEADZONE_STOP_MOVEMENT_ACCURACY
                )
            {
                moveTowardsCenterPoint = false;
            }
        }

        // Send tile map the info for activating/deactivating tiles that are (not)in view
        TileMapReference.ActivateTiles(new Vector2(LeftBound, TopBound), new Vector2(RightBound, BottomBound));
    }

    private void updateBounds()
    {
        // Top
        Vector3 pos = Bounds[(int)BOUNDS_TYPE.BOUNDS_TOP].transform.position;
        Vector3 scale = Bounds[(int)BOUNDS_TYPE.BOUNDS_TOP].transform.localScale;
        pos.y = TopBound;
        scale.x = ScreenData.GetScreenSize().x;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_TOP].transform.position = pos;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_TOP].transform.localScale = scale;

        // Bottom
        pos = Bounds[(int)BOUNDS_TYPE.BOUNDS_BOTTOM].transform.position;
        scale = Bounds[(int)BOUNDS_TYPE.BOUNDS_BOTTOM].transform.localScale;
        pos.y = BottomBound;
        scale.x = ScreenData.GetScreenSize().x;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_BOTTOM].transform.position = pos;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_BOTTOM].transform.localScale = scale;

        // Left
        pos = Bounds[(int)BOUNDS_TYPE.BOUNDS_LEFT].transform.position;
        scale = Bounds[(int)BOUNDS_TYPE.BOUNDS_LEFT].transform.localScale;
        pos.x = LeftBound;
        scale.y = ScreenData.GetScreenSize().y;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_LEFT].transform.position = pos;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_LEFT].transform.localScale = scale;

        // Right
        pos = Bounds[(int)BOUNDS_TYPE.BOUNDS_RIGHT].transform.position;
        scale = Bounds[(int)BOUNDS_TYPE.BOUNDS_RIGHT].transform.localScale;
        pos.x = RightBound;
        scale.y = ScreenData.GetScreenSize().y;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_RIGHT].transform.position = pos;
        Bounds[(int)BOUNDS_TYPE.BOUNDS_RIGHT].transform.localScale = scale;
    }
}
