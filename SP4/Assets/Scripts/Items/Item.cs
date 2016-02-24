using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;

    // Components
    protected Rigidbody2D rigidBody;
    protected new Collider2D collider;
    protected Animator anim;

    // Use this for initialization
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public abstract bool Use();
}
