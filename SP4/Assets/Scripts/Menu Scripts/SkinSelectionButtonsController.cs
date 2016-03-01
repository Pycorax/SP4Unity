#define DEBUG_BUTTONS
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectionButtonsController : MonoBehaviour
{

    [Tooltip("A reference to the SkinMenu that handles all interactions.")]
    public SkinMenu SkinMenuReference;

    [Tooltip("A reference to the button to use as a template to create other buttons.")]
    public Button TemplateSkinButton;

    [Tooltip("A reference to the SkinMenu that handles all interactions.")]
    public PlayerSettings PlayerSetting;

    [Tooltip("The horizontal distance between tile buttons.")]
    public float ButtonXMargin = 10;

#if DEBUG_BUTTONS
    public List<Skin> LoadSkins;
#endif


    // Components
    private RectTransform templateButtonRTf;

    // Use this for initialization
    void Start()
    {
#if DEBUG_BUTTONS
        foreach (var skin in LoadSkins)
        {
            skin.Load();
            PlayerSetting.AddToSkinStorage(skin);
        }
#endif

        Load();
    }

    public void Load()
    {
        // Get a reference to the template's rect transform for positioning later on
        templateButtonRTf = TemplateSkinButton.GetComponent<RectTransform>();

        // Save and use this to track the position to spawn a button next
        float buttonSpawnPosX = templateButtonRTf.rect.min.x + templateButtonRTf.rect.width * 0.5f;

        // Create a button for each tile in the Skin Inventory
        foreach (var skin in PlayerSetting.SkinsInventory)
        {
            // Create a button for this skin
            var skinButton = Instantiate(TemplateSkinButton);
            // Get a reference to the button's Button
            var tileButtonBtn = skinButton.GetComponent<Button>();
            // Get a reference to the button's Image
            var tileButtonImage = skinButton.GetComponent<Image>();
            // Get a reference tot he button's RectTransform
            var tileButtonRTf = skinButton.GetComponent<RectTransform>();

            // Register the Callback
            Skin thisSkin = skin;
            tileButtonBtn.onClick.AddListener(delegate { SkinMenuReference.SetCurrentPlayerSkin(thisSkin); });

            // Update the button's image
            tileButtonImage.sprite = skin.PreviewSprite;    

            // Make this a child of this controller
            skinButton.transform.SetParent(transform);

            // Set Position, Alignment and Scaling
            tileButtonRTf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (tileButtonRTf.parent.GetComponent<RectTransform>().rect.height - tileButtonRTf.rect.height) * 0.5f, templateButtonRTf.rect.height);
            tileButtonRTf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, buttonSpawnPosX, templateButtonRTf.rect.height);
            skinButton.transform.localScale = Vector3.one;

            // Update the next button spawn position
            buttonSpawnPosX += templateButtonRTf.rect.width + ButtonXMargin;
        }

        // Deactivate the now useless Template Button
        TemplateSkinButton.gameObject.SetActive(false);
    }
}
