using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Explosion : MonoBehaviour
{
    // Components
    private new Collider2D collider;

	// Use this for initialization
	void Start ()
    {
        // Set up Components
        collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    
}
