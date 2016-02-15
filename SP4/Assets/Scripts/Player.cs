using UnityEngine;

public class Player : MonoBehaviour
{
    public float Acceleration = 10.0f;
    public float Deceleration = 20.0f;
    public float MaxSpeed = 100.0f;
    public float MaxJumpTime = 0.25f; // Max time before jump accelerating stops

    // Controls
    public KeyCode MoveLeftKey = KeyCode.A;
    public KeyCode MoveRightKey = KeyCode.D;
    public KeyCode JumpKey = KeyCode.Space;

    // Movement
    private Vector2 prevDirection = Vector2.zero;

    // Jump
    private bool jumped = false; // Used to determine if player has jumped
    private bool jumpKeyPressed = false; // Register jump key being pressed
    private float jumpTimer = 0.0f; // Timer to allow player to choose how high they jump based on how long they press

    // Components
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        controlUpdate();
    }

    private void controlUpdate()
    {
        /*
         * Horizontal Movement
         */
        if (Input.GetKey(MoveLeftKey))
        {
            // Reset velocity if changing direction
            if (prevDirection != Vector2.left)
            {
                resetHorizontalVelocity();
            }

            moveTowards(Vector2.left);
            prevDirection = Vector2.left;
        }
        else if (Input.GetKey(MoveRightKey))
        {
            // Reset velocity if changing direction
            if (prevDirection != Vector2.right)
            {
                resetHorizontalVelocity();
            }

            moveTowards(Vector2.right);
            prevDirection = Vector2.right;
        }
        else
        {
            decelerate();
        }

        /*
         * Vertical Movement
         */
        if (Input.GetKey(JumpKey))
        {
            jumpKeyPressed = true;
            if (!jumped && jumpKeyPressed)
            {
                jump();
            }
        }
        else
        {
            if (jumpKeyPressed)
            {
                jumped = true;
            }
            jumpKeyPressed = false;
        }
    }

    private void moveTowards(Vector2 direction)
    {
        // Ensure that the direction passed in is a direction
        direction.Normalize();

        // Calculate the new velocity
        Vector2 newVelocity = rigidBody.velocity + direction * Acceleration * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

        // Clamp the velocity
        newVelocity.x = Mathf.Clamp(newVelocity.x, -MaxSpeed, MaxSpeed);

        rigidBody.velocity = newVelocity;
    }

    private void decelerate()
    {
        Vector2 deceleration = Vector2.zero;

        // Check what kind of deceleration to get
        if (rigidBody.velocity.x > 0)
        {
            deceleration.x = -Deceleration;
        }
        else if (rigidBody.velocity.x < 0)
        {
            deceleration.x = Deceleration;
        }

        // Calculate the new velocity
        Vector2 newVelocity = rigidBody.velocity + deceleration * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

        // Clamp the velocity
        if (rigidBody.velocity.x > 0)
        {
            newVelocity.x = Mathf.Clamp(newVelocity.x, 0.0f, MaxSpeed);
        }
        else if (rigidBody.velocity.x < 0)
        {
            newVelocity.x = Mathf.Clamp(newVelocity.x, -MaxSpeed, 0.0f);
        }

        rigidBody.velocity = newVelocity;

        // If the new velocity in the X is now 0, we reset the last recorded direction
        prevDirection = Vector2.zero;
    }

    private void resetHorizontalVelocity()
    {
        Vector2 newVel = rigidBody.velocity;
        newVel.x = 0.0f;
        rigidBody.velocity = newVel;
    }

    private void jump()
    {
        Vector2 newVel = rigidBody.velocity;
        newVel.y += 5000.0f * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        rigidBody.velocity = newVel;
        jumpTimer += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);
        if (jumpTimer >= MaxJumpTime)
        {
            endJump();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && collision.transform.position.y <= transform.position.y)
        {
            resetJump(); // Reset jump when landed
        }
    }

    private void endJump()
    {
        jumped = true;
        jumpTimer = 0.0f;
    }

    private void resetJump()
    {
        jumped = false;
        jumpTimer = 0.0f;
    }
}
