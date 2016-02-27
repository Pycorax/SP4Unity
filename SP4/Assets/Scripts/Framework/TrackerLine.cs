using UnityEngine;
using System.Collections;

public class TrackerLine : MonoBehaviour
{
    [Tooltip("The start point of the tracking line.")]
    public GameObject StartObject;
    [Tooltip("The end point of the tracking line.")]
    public GameObject EndObject;
    public Color LineColor = Color.white;
    public float Width = 1.0f;

    // Components
    private LineRenderer line;

	// Use this for initialization
	void Start ()
    {
        // Get a handle to the Line Renderer
        line = GetComponent<LineRenderer>();

        // Set up the Renderer
	    Init(StartObject, EndObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (StartObject != null && EndObject != null)
        {
            line.SetPosition(0, StartObject.transform.position);
            line.SetPosition(1, EndObject.transform.position);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(GameObject startObj, GameObject endObj)
    {
        StartObject = startObj;
        EndObject = endObj;

        line.SetPosition(0, StartObject.transform.position);
        line.SetPosition(1, EndObject.transform.position);
        line.SetWidth(Width, Width);
        line.material = new Material(Shader.Find("Unlit/Color"));
        line.material.color = LineColor;
        line.SetColors(LineColor, LineColor);
    }
}
