using UnityEngine;

public abstract class Destroyables : Item
{
    [Tooltip("A reference to a GameManager for statistics tracking.")]
    public GameManager Manager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
	}
	
	// Update is called once per frame
    protected override void Update()
    {
	
	}

	public override bool Use()
	{
        return true;
	}

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
