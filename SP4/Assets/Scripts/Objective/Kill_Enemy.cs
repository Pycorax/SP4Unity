using UnityEngine;
using System.Collections;

public class Kill_Enemy : Objectives
{
    private int enemies = 0;
    public int EnemiesToKill = 10;

    public override bool IsAchieved()
    {
        return (EnemiesToKill >= enemies);
    }

    public override void Complete()
    {

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
