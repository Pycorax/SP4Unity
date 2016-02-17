using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour {
    //Item Name
    public string ItemName;

    public string Name { get { return ItemName; } set { ItemName = value; } }

    public Animation animation;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public abstract void Use(Vector2 direction);
}
