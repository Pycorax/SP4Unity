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
    public bool IgnoreZ = false;

    // Components
    private LineRenderer line;

	// Use this for initialization
	void Start ()
    {
        // Get a handle to the Line Renderer
        line = GetComponent<LineRenderer>();

        // Set up the Renderer
	    if (StartObject != null && EndObject != null)
	    {
	        Init(StartObject, EndObject);
	    }
    }

    // Update is called once per frame
    void Update ()
    {
        if (StartObject != null && EndObject != null)
        {
            Vector3 startPos = StartObject.transform.position;
            Vector3 endPos = EndObject.transform.position;

            if (IgnoreZ)
            {
                startPos.z = endPos.z = transform.position.z;
            }

            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(GameObject startObj, GameObject endObj)
    {
        // Check if line was initialized
        if (line == null)
        {
            // If not, get a handle to the Line Renderer
            line = GetComponent<LineRenderer>();
        }

        StartObject = startObj;
        EndObject = endObj;

        Vector3 startPos = StartObject.transform.position;
        Vector3 endPos = EndObject.transform.position;

        if (IgnoreZ)
        {
            startPos.z = endPos.z = transform.position.z;
        }

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        line.SetWidth(Width, Width);
        line.material = new Material(Shader.Find("Unlit/Color"));
        line.material.color = LineColor;
        line.SetColors(LineColor, LineColor);
    }
}
