﻿using UnityEngine;

public class RPGPlayer : Character
{
    Inventory inventory;
    public float Acceleration = 3500.0f;
    public float Deceleration = 3000.0f;
    public float MaxSpeed = 500.0f;
    
    [Tooltip("The rotation offset from the original sprite direction to get the sprite to face right. This is used for calculating the correct direction of the player sprite.")]
    public float RotationSpriteOffset = -90.0f;
    [Tooltip("The delay time before currentWeapon will be reset.")]
    public float CurrentWeaponTimeDelay = 2.0f;

    // Player Attributes
    private int enemyKilled;
    private int coin;

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
    private float useTimeDelta;                         // The time since the last weapon attack

    // Components
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Getters
    public Weapon CurrentWeapon { get { return currentWeapon; } }
    public int EnemyKilled { get { return enemyKilled; } }


    //Projectile Controller
    public ProjectileManager ProjectileManager;

    // Use this for initialization
    protected override void Start()
    {
        // Base Start
        base.Start();

        // Initialize the Components
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        //healthBar = GetComponentInChildren<GameObject>();

        // Align the weapons properly if they exist
        if (LeftWeapon != null)
        {
            //alignWeapon(ref LeftWeapon, true);
            LeftWeapon.RefProjectileManager = ProjectileManager;
        }
        if (RightWeapon != null)
        {
            //alignWeapon(ref RightWeapon, false);
            RightWeapon.RefProjectileManager = ProjectileManager;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Base Update
        base.Update();

        // Updates
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

        //HealthBar Testing
        HealthBarUpdate(Health);
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
            alignWeapon(ref LeftWeapon, true);

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
            alignWeapon(ref RightWeapon, false);

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
        bool shot = false;

        // Shoot Left
        if (Input.GetKeyDown(LeftAttackKey))
        {
            if (LeftWeapon != null)
            {
                if(attack(LeftWeapon))
                {
                    shot = true;
                }
            }

        }

        // Shoot Right
        if (Input.GetKeyDown(RightAttackKey))
        {
            if (RightWeapon != null)
            {
                if(attack(RightWeapon))
                {
                    shot = true;
                }
            }
        }

        // If no shots were made, update the last shot timer
        if (!shot && currentWeapon != null)
        {
            // Update the timer
            useTimeDelta += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            // If it's been too long,
            if (useTimeDelta > CurrentWeaponTimeDelay)
            {
                // Current weapon is not so current anymore so we remove it
                currentWeapon = null;
                // Reset the timer for the next instance
                useTimeDelta = 0.0f;
            }
        }
    }

    public Weapon getCurrentActiveWeapon()
    {
        return currentWeapon;
    }

    /// <summary>
    /// Sets the alignment of the sprite on the left or right hand.
    /// </summary>
    /// <param name="w">The weapon to align.</param>
    /// <param name="left">To align left or right.</param>
    private void alignWeapon(ref Weapon w, bool left)
    {
        // Get the scale and Abs(x) to ensure we are working with consistent data
        Vector3 newScale = w.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x);

        // If right, then we flip the image
        if (!left)
        {
            newScale.x = -newScale.x;
        }

        // Set the new Scale
        w.transform.localScale = newScale;
    }

    /// <summary>
    /// Uses the weapon selected to attack.
    /// </summary>
    /// <param name="w">The weapon to use to attack.</param>
    /// <returns>Whether the attack was succssful.</returns>
    private bool attack(Weapon w)
    {
        if (w.Use(previousDir))
        {
            // Update the current weapon
            currentWeapon = w;

            return true;
        }

        return false;
    }

    #endregion
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        string name = other.gameObject.name;

        //Check wether the object is a weapon
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

        //Check if the items are destroyables
        if (other.gameObject.GetComponent<Destroyables>() != null)
        {

            if (other.gameObject.GetComponent<Exit>() != null)
            {
                Debug.Log("Exit");
            }
            else if (other.gameObject.GetComponent<Pot>() != null)
            {
                Debug.Log("Pot");
            }
            else if (other.gameObject.GetComponent<Table>() != null)
            {
                Debug.Log("Table");
            }
            else if (other.gameObject.GetComponent<SpikeTrap>() != null)
            {
                Injure(other.GetComponent<SpikeTrap>().dmg);
                Debug.Log("Health: " + health);
            }
            else if (other.gameObject.GetComponent<Coin>() != null)
            {
                coin += other.GetComponent<Coin>().CoinAmount;
                other.gameObject.SetActive(false);
                Debug.Log("Coin: " + coin);
            }
            else if (other.gameObject.GetComponent<Cannon>() != null)
            {
                Debug.Log("Cannon");
            }
            else if (other.gameObject.GetComponent<Box>() != null)
            {
                Debug.Log("Box");
            }
            else if (other.gameObject.GetComponent<Heart>() != null)
            {
                Heal(other.GetComponent<Heart>().Healing);
                other.gameObject.SetActive(false);
                Debug.Log("Health: " + health);
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
        Weapon weapon = other.gameObject.GetComponent<Weapon>();
        Projectile proj = other.gameObject.GetComponent<Projectile>();

        if (weapon != null)
        {
            currentWeapon.CombinedUse(weapon);
        }
        else if (proj != null)
        {
            // TODO: Pass in weapon
            currentWeapon.CombinedUse(null, proj);
        }
    }

}
