using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;

    // Components
    protected Animation anim;
    protected Rigidbody2D rigidBody;
    protected new Collider2D collider;

    // Use this for initialization
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public abstract bool Use();
}
