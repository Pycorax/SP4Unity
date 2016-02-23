using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Explosion : MonoBehaviour
{
    [Tooltip("The starting size of the explosion.")]
    public Vector2 StartSize;
    [Tooltip("The end size of the explosion.")]
    public Vector2 MaxSize;
    [Tooltip("The speed of the explosion.")]
    public float BoomSpeed;
    [Tooltip("The damage inflicted on the player.")]
    public int Damage;

    // Expansion
    private Vector2 expandDelta;        // The explosion "direction"

    // Components
    private new Collider2D collider;

	// Use this for initialization
	void Start ()
    {
        // Set up Components
        collider = GetComponent<Collider2D>();

        // Initialize the size
        transform.localScale = StartSize;

        // Initialize the explosion "direction"
        expandDelta = MaxSize - StartSize;
        if (expandDelta != Vector2.zero)
        {
            expandDelta.Normalize();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (transform.localScale.x < MaxSize.x && transform.localScale.y < MaxSize.y)
        {
            Vector2 newScale = transform.localScale;
            newScale += expandDelta * BoomSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
            transform.localScale = newScale;
        }
        else
        {
            // Once we become large enough, it's time to kill ourself
            transform.gameObject.SetActive(false);

            // Reset variables
            transform.localScale = StartSize;
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();

        // Damage
        if (enemy)
        {
            enemy.Injure(Damage);
        }
    }
}
