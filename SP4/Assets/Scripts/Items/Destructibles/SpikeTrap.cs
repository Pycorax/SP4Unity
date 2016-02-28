using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public int dmg = 30;
    public float AnimSpeed = 0.1f;

    // Use this for initialization
    void Start()
    {
        GetComponent<Animator>().speed = AnimSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<RPGPlayer>() != null)
        {
            other.gameObject.GetComponent<RPGPlayer>().Injure(dmg);
        }
    }
}