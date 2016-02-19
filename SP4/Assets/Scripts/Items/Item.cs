using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {
    //Item Name
    public string ItemName;

    public string Name { get { return ItemName; } set { ItemName = value; } }

    public Animation anim;
    public Rigidbody2D rigidBody;
    public Collision2D collision2D;
    //public Collider2D collider2D;

    // Use this for initialization
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collision2D = GetComponent<Collision2D>();
        //collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public abstract void Use(Vector2 direction);

    public abstract void CombineUse(Projectile projectile, Weapon weapon);
}
