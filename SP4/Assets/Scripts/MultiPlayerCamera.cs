using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCamera : MonoBehaviour
{
    [Tooltip("List of GameObjects that the camera focus on.")]
    public List<GameObject> PlayerList;
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

    // Static Constants
    private const int PLAYER_COUNT = 2;

    // Camera Snapping
    private float cameraSnapTimer = 0.0f;

    // Components
    private new Camera camera;

    // Debug
    public GameObject TestObject;

	// Use this for initialization
	void Start ()
    {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Error checking. This script only works on 2 players.
        if (PlayerList.Count != PLAYER_COUNT)
        {
            throw new UnityException("Too many players assigned to MultiPlayerCamera!");
        }

        // Get the half distance between the players
        Vector2 deltaPos = PlayerList[1].transform.position - PlayerList[0].transform.position;
        deltaPos *= 0.5f; // PLAYER_COUNT

        // Calculate the center point
        Vector3 centerPoint = (Vector2)PlayerList[0].transform.position + deltaPos;

        if (DeadZoneEnabled)
        {
            // Check if this point is outside the deadzone, if so, then we move
            Vector2 topLeftBound = new Vector2(transform.position.x - camera.orthographicSize * camera.aspect + LeftDeadZonePadding, transform.position.y + camera.orthographicSize - TopDeadZonePadding);
            Vector2 botRightBound = new Vector2(transform.position.x + camera.orthographicSize * camera.aspect - RightDeadZonePadding, transform.position.y - camera.orthographicSize + BottomDeadZonePadding);

            #region Debugging Code: Displays Deadzone on Scene

            TestObject.transform.localScale = new Vector3(botRightBound.x - topLeftBound.x, botRightBound.y - topLeftBound.y, 1);
            Vector3 testObjPos = new Vector3(topLeftBound.x + (botRightBound.x - topLeftBound.x) * 0.5f, botRightBound.y + (topLeftBound.y - botRightBound.y) * 0.5f, 1.0f);
            testObjPos.z = 1;
            TestObject.transform.position = testObjPos;

            #endregion

            // Only move if we are out of the bounds
            if (!
                (
                    topLeftBound.x < centerPoint.x && botRightBound.x > centerPoint.x
                    &&
                    topLeftBound.y > centerPoint.y && botRightBound.y < centerPoint.y
                 )
                 &&
                 cameraSnapTimer <= 0.0f
                )
            {
                // Increment the snapping timer
                cameraSnapTimer += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

                // Do not change the Z-axis
                centerPoint.z = transform.position.z;
                // Set the camera's position as a result
                transform.position = Vector3.Lerp(transform.position, centerPoint, cameraSnapTimer / CameraSnapTime);
            }
            else
            {
                // Reset the snap timer since we are not snapping
                cameraSnapTimer = 0.0f;
            }
        }
        else
        {
            // If we are not at the center point, move towards it
            if (transform.position != centerPoint)
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
            }
        }
    }
}
