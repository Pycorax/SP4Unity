﻿using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int Damage;
    public float Speed;

    // Owner
    private Weapon owner = null;
    public Weapon Owner { get { return owner; } set { owner = value; } }

    // Bounds
    public float LeftBound { get { return transform.position.x - transform.localScale.x * 0.5f; } }
    public float RightBound { get { return transform.position.x + transform.localScale.x * 0.5f; } }
    public float TopBound { get { return transform.position.y + transform.localScale.y * 0.5f; } }
    public float BottomBound { get { return transform.position.y - transform.localScale.y * 0.5f; } }

    // Use this for initialization
    protected virtual void Start()
    {
        Damage = 0;
        Speed = 0.0f;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
        // Disable itself if out of camera
        /*MultiPlayerCamera cam = Camera.main.GetComponent<MultiPlayerCamera>();
        Vector3 camMaxBound = new Vector3(cam.RightBound, cam.TopBound);
        Vector3 camMinBound = new Vector3(cam.LeftBound, cam.BottomBound);
        Vector3 pMaxBound = new Vector3(RightBound, TopBound);
        Vector3 pMinBound = new Vector3(LeftBound, BottomBound);
        if (pMinBound.x > camMaxBound.x || pMaxBound.x < camMinBound.x || pMinBound.y > camMaxBound.y || pMaxBound.y < camMinBound.y)
        {
            Disable();
        }*/
	}

    public virtual void Activate(Vector3 position, Quaternion rotation, Vector2 direction)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        MoveTowards(direction);
    }

    public void MoveTowards(Vector2 direction)
    {
        // Ensure that the direction passed in is a direction
        direction.Normalize();

        // Calculate the new velocity
        Vector2 newVelocity = direction * /*(float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game) **/ Speed;

        // Clamp the velocity
        //newVelocity.x = Mathf.Clamp(newVelocity.x, 0, Speed);

        GetComponent<Rigidbody2D>().velocity = newVelocity;
    }


    /*
     * FUNCTION: OnCollisionEnter2D
     * params: Collision2D collision
     * 
     * brief: Perform Collision functions
     */
    public virtual void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy.Enemy>() != null)
        {
            //If Collided with Enemy Unit
            //Reduce Enemy HP (currently no function for that)
            Enemy.Enemy enemy = collision.gameObject.GetComponent<Enemy.Enemy>();
        }
        else if(collision.gameObject.GetComponent<RPGPlayer>() != null)
        {
            //If Collided with other player
            //Combine Weapon Powerzz
            //RPGPlayer player = collision.gameObject.GetComponent<RPGPlayer>();
            //player.getCurrentActiveWeapon().CombineUse(this, collision.gameObject.GetComponent<RPGPlayer>().getCurrentActiveWeapon());
        }
        else
        {
            Disable();
        }
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }

}
