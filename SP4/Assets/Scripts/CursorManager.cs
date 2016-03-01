using UnityEngine;
using System.Collections;

public class CursorManager : MonoBehaviour {

    public Texture2D CursorTexture;

	// Use this for initialization
	void Start () {
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
