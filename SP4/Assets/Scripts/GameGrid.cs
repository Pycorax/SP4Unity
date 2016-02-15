#undef DEBUG

using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // Grid Size
    [Tooltip("The number of blocks along the width.")]
    public int BlocksInWidth = 9;
    [Tooltip("The number of blocks along the height. Set to '0' for script to auto calculate.")]
    public int BlocksInHeight = 0;

    // Calculated Scale of Blocks
    private Vector2 blockScale;

    // Storage of Blocks
    private List<List<GameObject>> listOfBlocks = new List<List<GameObject>>();

    // For Debugging
#if DEBUG
    public GameObject DebugBlackBlock;
    public GameObject DebugGrayBlock;
#endif

    // Use this for initialization
    void Start ()
    {
        // Calculate scale of the blocks
        blockScale.x = transform.lossyScale.x / BlocksInWidth;

        
        if (BlocksInHeight <= 0)
        {
            // Calculate the scale of the number of blocks in height
            blockScale.y = blockScale.x;
            BlocksInHeight = (int)((transform.lossyScale.y / blockScale.y) + 0.5f);
        }
        else
        {
            blockScale.y = transform.lossyScale.y / BlocksInHeight;
        }

#if DEBUG
        Vector2 debugStartPos = (Vector2)(transform.position - (transform.lossyScale * 0.5f)) + blockScale * 0.5f;     // Use bottom left corner as the start point
        bool lastWasBlack = false;

        // Show the current grid
        for (int height = 0; height < BlocksInHeight; ++height)
        {
            for(int width = 0; width < BlocksInWidth; ++width)
            {
                GameObject go;

                if (lastWasBlack)
                {
                    go = Instantiate(DebugGrayBlock);


                }
                else
                {
                    go = Instantiate(DebugBlackBlock);
                }

                go.transform.localScale = blockScale;
                go.transform.position = new Vector2(debugStartPos.x + width * blockScale.x, debugStartPos.y + height * blockScale.y);
                Debug.Log(go.transform.position);

                lastWasBlack = !lastWasBlack;
            }
        }
#endif
    }
	
	// Update is called once per frame
	void Update ()
    {

	}
}
