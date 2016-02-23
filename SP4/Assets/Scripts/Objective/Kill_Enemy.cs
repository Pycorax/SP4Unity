using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    private int enemies = 0;
    public int EnemiesToKill = 10;

    public override bool IsAchieved()
    {
        if(EnemiesToKill >= enemies)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Complete()
    {
        Debug.Log("Kill_Enemy Complete()");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (string.Equals(other.tag, "Enemy"))
        {
            enemies++;
            Destroy(other.gameObject);
        }
    }
}
