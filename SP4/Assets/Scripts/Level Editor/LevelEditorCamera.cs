using UnityEngine;
using System.Collections;

public class LevelEditorCamera : MonoBehaviour
{
    public KeyCode MoveLeftKey = KeyCode.LeftArrow;
    public KeyCode MoveRightKey = KeyCode.RightArrow;
    public KeyCode MoveUpKey = KeyCode.UpArrow;
    public KeyCode MoveDownKey = KeyCode.DownArrow;
    public float Speed = 500.0f;
    public EditorTileMap RefTileMap;

    public float TopBound { get { return transform.position.y + ScreenData.GetScreenSize().y * 0.5f; } }
    public float BottomBound { get { return transform.position.y - ScreenData.GetScreenSize().y * 0.5f; } }
    public float LeftBound { get { return transform.position.x - ScreenData.GetScreenSize().x * 0.5f; } }
    public float RightBound { get { return transform.position.x + ScreenData.GetScreenSize().x * 0.5f; } }

    // Use this for initialization
    void Start ()
    {
        if (RefTileMap)
        {
            transform.position = RefTileMap.CenterPoint;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move horizontally
	    if (Input.GetKey(MoveLeftKey))
        {
            // Move left
            transform.position += Vector3.left * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        }
        else if (Input.GetKey(MoveRightKey))
        {
            // Move right
            transform.position += Vector3.right * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        }

        // Move vertically
        if (Input.GetKey(MoveUpKey))
        {
            // Move up
            transform.position += Vector3.up * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        }
        else if (Input.GetKey(MoveDownKey))
        {
            // Move down
            transform.position += Vector3.down * Speed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        }

        if (RefTileMap && RefTileMap.Active)
        {
            float x = Mathf.Clamp(transform.position.x, RefTileMap.LeftBound, RefTileMap.RightBound);
            float y = Mathf.Clamp(transform.position.y, RefTileMap.BottomBound, RefTileMap.TopBound);
            transform.position = new Vector3(x, y);
            RefTileMap.ActivateTiles(new Vector3(LeftBound, TopBound), new Vector3(RightBound, BottomBound));
        }
	}
}
