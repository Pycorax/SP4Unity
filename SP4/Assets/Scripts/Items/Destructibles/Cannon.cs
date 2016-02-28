using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public float AnimSpeed = 0.5f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Animator>().speed = AnimSpeed;
    }
}
