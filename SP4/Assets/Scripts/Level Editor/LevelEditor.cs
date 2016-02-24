using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEditor : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GetComponent<CanvasScaler>().referenceResolution = ScreenData.GetScreenSize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y)));
	}
}
