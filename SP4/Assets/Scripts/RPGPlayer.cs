using UnityEngine;

public class RPGPlayer : MonoBehaviour
{
    Inventory inventory = new Inventory();
    public float Acceleration = 3500.0f;
    public float Deceleration = 3000.0f;
    public float MaxSpeed = 500.0f;
    [Tooltip("The maximum health of the character.")]
    public int MaxHealth = 100;

    // Player Attributes
    private int health;

    // Controls
    public KeyCode MoveLeftKey = KeyCode.A;
    public KeyCode MoveRightKey = KeyCode.D;
    public KeyCode MoveUpKey = KeyCode.W;
    public KeyCode MoveDownKey = KeyCode.S;
    public KeyCode LeftAttacKey = KeyCode.Q;
    public KeyCode RightAttackKey = KeyCode.E;

    // Movement
    private Vector2 prevHoriDir = Vector2.zero;         // Determines the direction that was last pressed in the horizontal
    private Vector2 prevVertDir = Vector2.zero;         // Determines the direction that was last pressed in the vertical         
    private Vector2 previousDir = Vector2.up;           // Stores the current direction of the player

    // Weapons
    private Weapon LeftWeapon;
    private Weapon RightWeapon;

    // Components
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    //Weapon
    private Weapon weapon_1; //Left Hand
    private Weapon weapon_2; //Right Hand

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movementUpdate();

        // Update the direction of the player
        if (rigidBody.velocity != Vector2.zero)
        {
            previousDir = rigidBody.velocity.normalized;
            transform.localEulerAngles = new Vector3(0.0f, 0.0f, Vector2.Angle(Vector2.up, previousDir));
            Debug.Log(previousDir);
        }
    }

    #region Movement

    private void movementUpdate()
    {
        bool horiMoved = false;
        bool vertMoved = false;

        /*
         * Horizontal Movement
         */
        if (Input.GetKey(MoveLeftKey))
        {
            // Reset velocity if changing direction
            if (prevHoriDir != Vector2.left)
            {
                resetVelocity(true);
            }

            moveTowards(Vector2.left);
            prevHoriDir = Vector2.left;
            horiMoved = true;
        }
        else if (Input.GetKey(MoveRightKey))
        {
            // Reset velocity if changing direction
            if (prevHoriDir != Vector2.right)
            {
                resetVelocity(true);
            }

            moveTowards(Vector2.right);
            prevHoriDir = Vector2.right;
            horiMoved = true;
        }

        /*
         * Vertical Movement
         */
        if (Input.GetKey(MoveUpKey))
        {
            // Reset velocity if changing direction
            if (prevVertDir != Vector2.up)
            {
                resetVelocity(false);
            }

            moveTowards(Vector2.up);
            prevVertDir = Vector2.up;
            vertMoved = true;
        }
        else if (Input.GetKey(MoveDownKey))
        {
            // Reset velocity if changing direction
            if (prevVertDir != Vector2.down)
            {
                resetVelocity(false);
            }

            moveTowards(Vector2.down);
            prevVertDir = Vector2.down;
            vertMoved = true;
        }

        deceleration(horiMoved, vertMoved);

        // Clamp the velocity
        if (rigidBody.velocity.SqrMagnitude() > MaxSpeed * MaxSpeed)
        {
            Vector2 newVel = rigidBody.velocity;
            newVel = newVel.normalized * MaxSpeed;
            rigidBody.velocity = newVel;
        }
        else
        {
            // If the new velocity in either axis is now 0, we reset the last recorded direction in that axis
            if (rigidBody.velocity.x == 0.0f)
            {
                prevHoriDir = Vector2.zero;
            }

            if (rigidBody.velocity.y == 0.0f)
            {
                prevVertDir = Vector2.zero;
            }
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

    private void deceleration(bool movedHori, bool moveVert)
    {
        // Calculate the deceleration direction
        Vector2 decelerationDir = -rigidBody.velocity;
        if (decelerationDir == Vector2.zero)
        {
            return;
        }
        decelerationDir.Normalize();

        // Calculate the deceleration this frame
        Vector2 currDecel = (decelerationDir * Deceleration * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
        if (currDecel.x > Mathf.Abs(rigidBody.velocity.x))
        {
            currDecel.x = -rigidBody.velocity.x;
        }
        if (currDecel.y > Mathf.Abs(rigidBody.velocity.y))
        {
            currDecel.y = -rigidBody.velocity.y;
        }

        // Calculate the new velocity
        Vector2 newVelocity = rigidBody.velocity + currDecel;
        rigidBody.velocity = newVelocity;
    }

    private void resetVelocity(bool horizontal)
    {
        Vector2 newVel = rigidBody.velocity;
        if (horizontal)
        {
            newVel.x = 0.0f;
        }
        else
        {
            newVel.y = 0.0f;
        }
        rigidBody.velocity = newVel;
    }


    #endregion

    #region Attack / Weapons
        
    private void attackUpdate()
    {
        
    } 

    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        string name = other.gameObject.name;

        if (other.gameObject.GetComponent<Weapon>() != null)
        {
            Weapon w = other.gameObject.GetComponent<Weapon>();
            if (w != null)
            {
                inventory.AddItem(w);
            }
        }
    }
}
