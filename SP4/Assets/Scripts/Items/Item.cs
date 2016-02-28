using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;

    // Components
    protected Animator anim;

    // Use this for initialization
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    public virtual void Deactivate()
    {
        Tile tile = GetComponent<Tile>();
        if (tile)
        {
            tile.IgnoreActive = true;
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public abstract bool Use();
}
