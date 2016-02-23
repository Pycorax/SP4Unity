using UnityEngine;
using System.Collections;

public abstract class Objectives : MonoBehaviour {

    public RPGPlayer player;

    public abstract bool IsAchieved();
    public abstract void Complete();
}
 