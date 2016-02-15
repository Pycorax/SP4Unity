using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // Grid Size
    [Tooltip("The number of blocks along the width.")]
    public int BlocksInWidth = 9;
    [Tooltip("The number of blocks along the height.")]
    public int BlocksInHeight = 18;

    // Calculated Scale of Blocks
    private Vector2 blockScale;

    // Storage of Blocks
    private List<List<GameObject>> listOfBlocks = new List<List<GameObject>>();

	// Use this for initialization
	void Start ()
    {
        // Get scale of the grid
        Vector2 scale = transform.lossyScale;


	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
