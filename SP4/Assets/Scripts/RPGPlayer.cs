using UnityEngine;

public class RPGPlayer : MonoBehaviour
{
    Inventory inventory;
    public float Acceleration = 3500.0f;
    public float Deceleration = 3000.0f;
    public float MaxSpeed = 500.0f;
    [Tooltip("The maximum health of the character.")]
    public int MaxHealth = 100;
    [Tooltip("The rotation offset from the original sprite direction to get the sprite to face right. This is used for calculating the correct direction of the player sprite.")]
    public float RotationSpriteOffset = -90.0f;

    // Player Attributes
    private int health;
    private int enemyKilled;
    private int coin;

    //Destructable Object
    public int spikeDamage = 20;
    public int healthHeal = 30;
    public int coinAdd = 1;

    // Weapons
    public Weapon LeftWeapon;
    public Weapon RightWeapon;

    // Controls
    public bool UseMouseControl = false;
    public KeyCode MoveLeftKey = KeyCode.A;
    public KeyCode MoveRightKey = KeyCode.D;
    public KeyCode MoveUpKey = KeyCode.W;
    public KeyCode MoveDownKey = KeyCode.S;
    public KeyCode LeftAttackKey = KeyCode.Q;
    public KeyCode RightAttackKey = KeyCode.E;

    // Movement
    private Vector2 prevHoriDir = Vector2.zero;         // Determines the direction that was last pressed in the horizontal
    private Vector2 prevVertDir = Vector2.zero;         // Determines the direction that was last pressed in the vertical         
    private Vector2 previousDir = Vector2.up;           // Stores the current direction of the player
    private const float MOUSE_CONTROL_DEADZONE = 5.0f;

    // Weapons
    private Weapon currentWeapon;                       // Stores a reference to the last weapon used by the player. For use with combo attacks.

    // Components
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    // Getters
    public int Health { get { return health; } }
    public Weapon CurrentWeapon { get { return currentWeapon; } }
    public int EnemyKilled { get { return enemyKilled; } }
    public int Coin { get { return coin; } }


    //Projectile Controller
    public ProjectileManager ProjectileManager;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        movementUpdate();
        attackUpdate();

        // Update the direction of the player
        if (rigidBody.velocity != Vector2.zero)
        {
            // Get the directional unit vector
            previousDir = rigidBody.velocity.normalized;
            // Calculate the angle using Atan2 and add RotationSpriteOffset due to realign with original sprite direction
            float angle = Mathf.Atan2(previousDir.y, previousDir.x) * Mathf.Rad2Deg + RotationSpriteOffset;
            // Set the rotation according to a calculation based on the angle
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    /// <summary>
    /// Initialization function for a RPGPlayer
    /// </summary>
    /// <param name="startPosition"></param>
    public void Init(Vector3 startPosition)
    {
        transform.position = startPosition;
    }

    #region Movement

    private void movementUpdate()
    {
        bool horiMoved = false;
        bool vertMoved = false;

        if (UseMouseControl)
        {

            mouseMovementInput(out horiMoved, out vertMoved);
        }
        else
        {
            keyboardMovementInput(out horiMoved, out vertMoved);
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

    private void keyboardMovementInput(out bool horiMoved, out bool vertMoved)
    {
        // Set default values
        horiMoved = vertMoved = false;

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
    }

    private void mouseMovementInput(out bool horiMoved, out bool vertMoved)
    {
        // Set default values
        horiMoved = vertMoved = false;

        // Get the direction to move in
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        // Only move if we are outside the dead zone
        if (dir.sqrMagnitude > MOUSE_CONTROL_DEADZONE * MOUSE_CONTROL_DEADZONE)
        {
            moveTowards(dir);

            // Check for horizontal movement
            if (Mathf.Abs(dir.x) > 0.0f)
            {
                horiMoved = true;
            }
            // Check for vertical movement
            if (Mathf.Abs(dir.y) > 0.0f)
            {
                vertMoved = true;
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

    /// <summary>
    /// Use this function to equip weapons on the left hand
    /// </summary>
    /// <param name="weap">The weapon to equip</param>
    /// <returns>Whether the equip process was successful</returns>
    public bool EquipLeftHand(Weapon weap)
    {
        // Store the weapon on the right hand
        storeWeapon(ref LeftWeapon);

        // If we can store the item
        if (LeftWeapon == null)
        {
            LeftWeapon = weap;
            LeftWeapon.transform.rotation = transform.rotation;
            LeftWeapon.transform.position = transform.position;
            LeftWeapon.transform.localScale = transform.localScale;
            LeftWeapon.transform.parent = transform;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Use this function to equip weapons on the right hand
    /// </summary>
    /// <param name="weap">The weapon to equip</param>
    /// <returns>Whether the equip process was successful</returns>
    public bool EquipRightHand(Weapon weap)
    {
        // Store the weapon on the right hand
        storeWeapon(ref RightWeapon);

        // If we can store the item
        if (RightWeapon == null)
        {
            RightWeapon = weap;
            RightWeapon.transform.rotation = transform.rotation;
            RightWeapon.transform.position = transform.position;
            //LeftWeapon.transform.localScale = transform.localScale;
            RightWeapon.transform.parent = transform;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Use this function to equip weapons on a free hand. It will find a hand that is free and equip the item on it. Right hand is prioritized.
    /// </summary>
    /// <param name="weap">The weapon to equip</param>
    /// <returns>Whether the equip process was successful</returns>
    public bool EquipHand(Weapon weap)
    {

        // Try to equip right
        if (RightWeapon == null)
        {
            return EquipRightHand(weap);
        }
        // Try to equip left
        else if (LeftWeapon == null)
        {
            return EquipLeftHand(weap);
        }

        // Nope, both hands are filled
        return false;
    }
    /// <summary>
    /// Store the weapon on the Left Hand
    /// </summary>
    /// <returns>Whether storing was successful.</returns>
    public bool StoreLeftHand()
    {
        return storeWeapon(ref LeftWeapon);
    }

    /// <summary>
    /// Store the weapon on the Right Hand
    /// </summary>
    /// <returns>Whether storing was successful.</returns>
    public bool StoreRightHand()
    {
        return storeWeapon(ref RightWeapon);
    }

    /// <summary>
    /// Use this function to store weapons. It is a helper function for EquipLeftHand() and EquipRightHand().
    /// </summary>
    /// <param name="weap">The "hand" holding the weapon to store</param>
    private bool storeWeapon(ref Weapon hand)
    {
        // Attempt to store existing weapon if currently holding one
        if (hand != null)
        {
            // Store the item in
            if (inventory.AddItem(hand))
            {
                // Clear the hand
                hand = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void attackUpdate()
    {
        if (Input.GetKeyDown(LeftAttackKey))
        {
            if (LeftWeapon != null)
            {
                if (LeftWeapon.FireRate == 0)
                {
                    Debug.Log("using Left Weapon");
                    currentWeapon = LeftWeapon;
                    //LeftWeapon.Use(previousDir);
                }
            }

        }

        if (Input.GetKeyDown(RightAttackKey))
        {
            if (RightWeapon != null)
            {
                // TODO: Right Attack
                if (RightWeapon.FireRate == 0)
                {
                    Debug.Log("using Right Weapon");
                    currentWeapon = RightWeapon;
                    //RightWeapon.Use(previousDir);
                }
            }
        }
        else
        {
            currentWeapon = null;
        }
    }

    public Weapon getCurrentActiveWeapon()
    {
        return currentWeapon;
    }
    #endregion

    #region Health

    void Injure(int damage)
    {
        // Error checks
        if (damage < 0)
        {
            throw new UnityException("Please don't use Injure() to heal!");
        }

        health -= damage;

        // Clamp the health so we don't go crazy with the health accidentally
        Mathf.Clamp(health, 0, MaxHealth);
    }

    void Heal(int healing)
    {
        // Error checks
        if (healing < 0)
        {
            throw new UnityException("Please don't use Heal() to injure!");
        }

        health += healing;

        // Clamp the health so we don't go crazy with the health accidentally
        Mathf.Clamp(health, 0, MaxHealth);
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
                EquipHand(w);
            }
            else if (!EquipHand(w) && w != null)
            {
                inventory.AddItem(w);
            }
        }

        if (other.gameObject.GetComponent<Destroyables>() != null)
        {
            string d = other.gameObject.name;

            switch (d)
            {
                case "Exit":
                    break;
                case "Pot":
                    break;
                case "Table":
                    break;
                case "SpikeTrap":
                    health -= spikeDamage;
                    Debug.Log(health);
                    break;
                case "Coin":
                    coin += coinAdd;
                    other.gameObject.SetActive(false);
                    Debug.Log(coin);
                    break;
                case "Cannon":
                    break;
                case "Box":
                    break;
                case "Heart":
                    health += healthHeal;
                    other.gameObject.SetActive(false);
                    Debug.Log(health);
                    break;
            }

        }

        //if (other.gameObject.GetComponent<ObjectiveStart>() != null && ObjectiveStarted == false)
        //{
        //    Debug.Log("Objective Start");
        //    ObjectiveStarted = true;
        //}

        //else if (other.gameObject.GetComponent<ObjectiveEnd>() != null && ObjectiveStarted == true)
        //{
        //    Debug.Log("Objective End");
        //    ObjectiveStarted = false;
        //}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }
}
