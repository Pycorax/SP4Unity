using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileSelectedEvent : UnityEvent<Tile> { } //empty class; just needs to exist



public class LevelEditorButtonsController : MonoBehaviour
{
    [Tooltip("A reference to the button to use as a template to create other buttons.")]
    public Button TemplateTileButton;

    [Tooltip("A reference to the TileSet that contains all the tile types.")]
    public GameObject TileSet;

    [Tooltip("The vertical distance between tile buttons.")]
    public float TileButtonYMargin = 10;

    [Tooltip("A reference to the LevelEditor you wish to work with.")]
    public LevelEditorGemini LevelEditorReference;

    // Components
    private RectTransform templateTileButtonRTf;

    // Use this for initialization
    void Start ()
	{
        // Get a reference to the template tile's rect transform for positioning later on
	    templateTileButtonRTf = TemplateTileButton.GetComponent<RectTransform>();

        // Save and use this to track the position to spawn a button next
        float buttonSpawnPosY = templateTileButtonRTf.rect.min.y + templateTileButtonRTf.rect.height * 0.5f;

        // Create a button for each tile in the TileSet
	    foreach (var t in TileSet.GetComponentsInChildren<Tile>())
	    {
            // Create a tile button for this guy
	        var tileButton = Instantiate(TemplateTileButton);
            // Get a reference to the button's Button
	        var tileButtonBtn = tileButton.GetComponent<Button>();
            // Get a reference to the button's Image
	        var tileButtonImage = tileButton.GetComponent<Image>();
            // Get a reference tot he button's RectTransform
            var tileButtonRTf = tileButton.GetComponent<RectTransform>();

            // Register the Callback
	        Tile thisTile = t;
            tileButtonBtn.onClick.AddListener(delegate { LevelEditorReference.TileSelected(thisTile); });

            // Update the button's image
	        tileButtonImage.sprite = t.GetComponent<SpriteRenderer>().sprite;

            // Make this a child of this controller
	        tileButton.transform.SetParent(transform);
        
            // Set Position, Alignment and Scaling
            tileButtonRTf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, (tileButtonRTf.parent.GetComponent<RectTransform>().rect.width - tileButtonRTf.rect.width) * 0.5f, templateTileButtonRTf.rect.width);
            tileButtonRTf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, buttonSpawnPosY, templateTileButtonRTf.rect.height);
	        tileButton.transform.localScale = Vector3.one;

            // Update the next button spawn position
            buttonSpawnPosY += templateTileButtonRTf.rect.height + TileButtonYMargin;
	    }

        // Deactivate the now useless TemplateTileButton
        TemplateTileButton.gameObject.SetActive(false);
	}
}
