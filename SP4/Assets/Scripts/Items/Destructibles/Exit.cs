using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{
    [Tooltip("A reference to a GameManager for statistics tracking.")]
    public GameManager Manager;

    private Animator anim;

    public float AnimSpeed = 0.5f;

    // For tracking leave
    private bool playerIsIn = false;
    private float timeSincePlayerEntered = 0.0f;
    private const float MAX_TIME_BEFORE_LEAVE = 0.5f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = AnimSpeed;
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If player is marked as entered, we start tracking if player had left
        if (playerIsIn)
        {
            // Update the leave timer
            timeSincePlayerEntered += (float) TimeManager.GetDeltaTime(TimeManager.TimeType.Game);

            // If player has left too long, then we inform the Game Manager
            if (timeSincePlayerEntered > MAX_TIME_BEFORE_LEAVE)
            {
                OnLeave();
            }
        }
        Manager.NotifyLeftExit();
    }

    public void Onhit()
    {
        anim.enabled = true;

        // Track player enter/leave
        playerIsIn = true;
        timeSincePlayerEntered = 0.0f;

        // Notify reached exit
        Manager.NotifyReachedExit();
    }

    public void OnLeave()
    {
        // Keep track ourselves
        playerIsIn = false;

        // Notify left exit
        Manager.NotifyLeftExit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RPGPlayer player;
        if (player = other.gameObject.GetComponent<RPGPlayer>())
        {
            Manager.NotifyReachedExit();
        }
    }
}
