using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("The list of background starting from the bottom to the top.")]
    public List<Sprite> BackgroundList;
    [Tooltip("The multiplier for the speed at which the background moves.")]
    public float ScrollMultiplier;

    // References to the two backgrounds so that we may scroll and transition seamlessly
    public GameObject CurrentBackground;
    public GameObject NextBackground;

    // Keeps track of the current background

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Scrolling Update

        // Move the two BGs

    }
}
