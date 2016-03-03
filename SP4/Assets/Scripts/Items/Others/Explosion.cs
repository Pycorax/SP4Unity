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
	}

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject.GetComponent<Enemy.Enemy>();

        // Damage
        if (enemy)
        {
            enemy.Injure(Damage);
        }
    }
}
