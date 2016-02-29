﻿using UnityEngine;
using UnityEngine.UI;

public class GachaponScreen : MonoBehaviour
{
    [Tooltip("The GameObject that holds the Merchant items. This will be hidden during the transition.")]
    public GameObject MerchantContainer;
    [Tooltip("The speed at which the MerchantContainer fades away.")]
    public float FadeSpeed = 10.0f;
    [Tooltip("The GameObject that holds the Capsule items. This will be hidden at the start until the transition.")]
    public GameObject CapsuleContainer;

    // State of this Screen
    private enum State
    {
        Vanilla,
        HideMerch,
        ShowCapsule,
        OpenCapsule,
        End
    }

    // Animation
    private State menuState = State.Vanilla;
    private int animGachaOpen = Animator.StringToHash("OpenGacha"); // The hash for the Capsule animator's OpenGacha trigger

    // Components
    // -- Merchant Container
    private Image[] merchantChildImages;
    private Text[] merchantChildText;
    // -- Capsule Container
    private Image capsuleImage;
    private Animator capsuleAnimator;

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

        // Set Up the Scene
        CapsuleContainer.SetActive(false);
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

                foreach (var txt in merchantChildText)
                {
                    // Set the transparency of each text
                    setColorAlpha(txt, txt.color.a - FadeSpeed * (float)TimeManager.GetDeltaTime(TimeManager.TimeType.Game));
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
                    capsuleAnimator.SetTrigger(animGachaOpen);
                    menuState = State.OpenCapsule;
                }
                break;

            case State.OpenCapsule:
                
                break;
        }
	}

    public void BuyGachapon()
    {
        menuState = State.HideMerch;
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
}
