using UnityEngine;
using System.Collections;

public abstract class Objectives : MonoBehaviour {

    public RPGPlayer player;

    public virtual void Start()
    {

    }
    public abstract bool IsAchieved();
    public abstract void Complete();
}
 