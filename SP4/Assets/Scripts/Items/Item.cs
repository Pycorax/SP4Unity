using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;

    // Components
    protected Animator anim;

    // Use this for initialization
    protected virtual void Start()
    {
        Tile tile = GetComponent<Tile>();
        if (tile)
        {
            tile.IgnoreActive = true;
        }
        anim = GetComponent<Animator>();
    }

    public virtual void Deactivate()
    {
        Tile tile = GetComponent<Tile>();
        if (tile)
        {
            tile.IgnoreActive = true;
        }
        ScoreManager.AddCurrentScore(ScoreManager.ScoreType.Destructables);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public abstract bool Use();
}
