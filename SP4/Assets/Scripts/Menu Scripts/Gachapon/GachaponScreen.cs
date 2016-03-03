using UnityEngine;
using UnityEngine.UI;

public class GachaponScreen : MonoBehaviour
{
    [Tooltip("The GameObject that holds the Merchant items. This will be hidden during the transition.")]
    public GameObject MerchantContainer;
    [Tooltip("The speed at which the MerchantContainer fades away.")]
    public float FadeSpeed = 10.0f;
    [Tooltip("The GameObject that holds the Reward item.")]
    public GameObject Reward;
    [Tooltip("The Reward GameObject. This will be hidden at the start until the transition.")]
    public GameObject RewardContainer;
    [Tooltip("The Capsule GameObject. This will be hidden at the start until the transition.")]
    public GameObject CapsuleContainer;
    [Tooltip("Reference to the Gachapon machine.")]
    public Gachapon GachaponMachine;
    [Tooltip("Reference to the Player to get the player's number of coins.")]
    public PlayerSettings PlayerSetting;

    // State of this Screen
    private enum State
    {
        Vanilla,
        HideMerch,
        ShowCapsule,
        OpenCapsule,
        ShowEnd,
        End
    }

    // Reward
    private Skin rewardSkin;                                        // The skin that the Gachapon gives to the player.

    // Animation
    private State menuState = State.Vanilla;
    private int animGachaOpen = Animator.StringToHash("OpenGacha"); // The hash for the Capsule animator's OpenGacha trigger
    private int animRewardShow = Animator.StringToHash("Appear");   // The hash for the Reward animator's Appear trigger
    private int animReset = Animator.StringToHash("Reset");         // The hash for animators' Reset triggers

    // Components
    // -- Merchant Container
    private Image[] merchantChildImages;
    private Text[] merchantChildText;
    // -- Capsule Container
    private Image capsuleImage;
    private Animator capsuleAnimator;
    // -- Reward Container
    private Animator rewardAnimator;
    private Image rewardImage;
    private Image[] rewardChildImages;
    private Text[] rewardChildTexts;

    // Use this for initialization
    void Start ()
    {
        // Get References for State.HideMerch
        merchantChildImages = MerchantContainer.GetComponentsInChildren<Image>();
        merchantChildText = MerchantContainer.GetComponentsInChildren<Text>();

        // Get References for State.ShowCapsule
        capsuleImage = CapsuleContainer.GetComponentInChildren<Image>();

        // Get References for State.OpenCapsule
        capsuleAnimator = CapsuleContainer.GetComponentInChildren<Animator>();
        rewardAnimator = Reward.GetComponent<Animator>();
        rewardImage = Reward.GetComponent<Image>();

        // Get References for State.EndShow
        rewardChildImages = RewardContainer.GetComponentsInChildren<Image>();
        rewardChildTexts = RewardContainer.GetComponentsInChildren<Text>();

        // Set Up the Scene
        reset(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (menuState)
        {
            case State.HideMerch:
                /*
                 * Gradually make everything invisible
                 */
                foreach (var img in merchantChildImages)
                {
                    // Set the transparency of each image
                    setColorAlpha(img, img.color.a - FadeSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
                }

                // If everything is invisible; Transition to the next scene
                if (merchantChildImages.Length > 0 && merchantChildImages[0].color.a <= 0.0f)
                {
                    // Disable the Merchant Container
                    MerchantContainer.SetActive(false);

                    // Enable the Capsule Container for the next state
                    CapsuleContainer.SetActive(true);

                    // Go to the next state
                    menuState = State.ShowCapsule;
                }
                break;

            case State.ShowCapsule:
                if (capsuleImage.color.a < 1.0f)
                {
                    setColorAlpha(capsuleImage, capsuleImage.color.a + FadeSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
                }
                else
                {
                    // Go to the next state and play the unveiling anomation
                    capsuleAnimator.SetTrigger(animGachaOpen);
                    rewardAnimator.SetTrigger(animRewardShow);

                    menuState = State.OpenCapsule;
                }
                break;

            case State.OpenCapsule:
                break;

            case State.ShowEnd:
                /*
                 * Gradually make reward text and images visible
                 */
                foreach (var img in rewardChildImages)
                {
                    // Set the transparency of each image
                    setColorAlpha(img, img.color.a + FadeSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
                }

                foreach (var text in rewardChildTexts)
                {
                    // Set the transparency of each image
                    setColorAlpha(text, text.color.a + FadeSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
                }

                // Finish after ending the animation
                if (rewardChildImages.Length > 0 && rewardChildImages[0].color.a >= 255.0f)
                {
                    menuState = State.End;
                }
                break;
        }
	}

    public void BuyGachapon()
    {
        // Check if there is not enough cash
        if (!PlayerSetting.TakeCoins(GachaponMachine.Cost))
        {
            // Do not allow buying
            return;
        }

        // Generate the skin
        rewardSkin = GachaponMachine.GetRandomSkin();

        if (rewardSkin != null)
        {
            // Set the reward image
            rewardImage.sprite = rewardSkin.PreviewSprite;

            // Play the animation
            startAnimation();

            // Store the skin
            PlayerSetting.SkinsInventory.Add(rewardSkin);
        }
    }

    /// <summary>
    /// Use this function at the end and finish up, reset.
    /// </summary>
    public void FinishGacha()
    {
        reset();
    }

    /// <summary>
    /// Use this function in the Animator to inform us that the capsule has finished being opened
    /// </summary>
    public void NotifyFinishedOpeningCapsule()
    {
        if (menuState == State.OpenCapsule)
        {
            menuState = State.ShowEnd;
        }
    }

    private void startAnimation()
    {
        // Kickstart the animation
        menuState = State.HideMerch;

        // Hide the text
        foreach (var txt in merchantChildText)
        {
            txt.gameObject.SetActive(false);
        }
    }

    private void reset(bool initial = false)
    {
        // Reset the State
        menuState = State.Vanilla;

        // Reset the skin
        rewardSkin = null;

        // Reset all the items in the scene
        // -- Images
        foreach (var img in merchantChildImages)
        {
            // Set the transparency of each image
            setColorAlpha(img, 1.0f);
        }
        foreach (var img in rewardChildImages)
        {
            // Set the transparency of each image
            setColorAlpha(img, 0.0f);
        }
        // -- Text
        foreach (var txt in merchantChildText)
        {
            txt.gameObject.SetActive(true);
        }
        foreach (var txt in rewardChildTexts)
        {
            setColorAlpha(txt, 0.0f);
        }

        // -- Animations
        if (!initial)
        {
            // ---- Ensure that they are enabled so that we can set them
            CapsuleContainer.SetActive(true);
            RewardContainer.SetActive(true);
            // ---- Do the actual reset
            rewardAnimator.SetTrigger(animReset);
            capsuleAnimator.SetTrigger(animReset);
        }

        // -- Parent Containers
        CapsuleContainer.SetActive(false);
        MerchantContainer.SetActive(true);
    }

    private void setColorAlpha(Image imageToSet, float finalAlpha)
    {
        Color newCol = imageToSet.color;
        newCol.a = finalAlpha;
        imageToSet.color = newCol;
    }

    private void setColorAlpha(Text txtToSet, float finalAlpha)
    {
        Color newCol = txtToSet.color;
        newCol.a = finalAlpha;
        txtToSet.color = newCol;
    }

    public void ReturnToMenu()
    {
        // Save settings before leaving
        PlayerSetting.Save();

        Application.LoadLevel("MainMenuScene");
    }
}
