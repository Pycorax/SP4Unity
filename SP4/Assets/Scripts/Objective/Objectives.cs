using UnityEngine;
using System.Collections;

public abstract class Objectives : MonoBehaviour {

    public enum Type
    {
        KillAll,
        CollectCoins,
        NoDamage
    }

    public RPGPlayer player;

    public virtual void Start()
    {

    }
    public abstract bool IsAchieved();
    public abstract void Complete();
    public virtual void Update()
    {

    }
}
 