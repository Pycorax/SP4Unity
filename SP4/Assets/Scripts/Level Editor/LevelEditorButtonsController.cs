using UnityEngine;
using UnityEngine.UI;


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

    [Tooltip("If true, this is treated as a template and Start() will not load the buttons.")]
    public bool IsTemplate = false;

    [Tooltip("The transparency level of tiles buttons which have no colliders on the tiles.")]
    public float PassThroughTileAlphaLevel = 0.75f;

    // Components
    private RectTransform templateTileButtonRTf;

    // Use this for initialization
    void Start ()
	{
        if (!IsTemplate)
        {
            Load(TileSet);
        }
	}

    public void Load(GameObject tileSet)
    {
        // Get a reference to the template tile's rect transform for positioning later on
        templateTileButtonRTf = TemplateTileButton.GetComponent<RectTransform>();

        // Save and use this to track the position to spawn a button next
        float buttonSpawnPosY = templateTileButtonRTf.rect.min.y + templateTileButtonRTf.rect.height * 0.5f;

        // Create a button for each tile in the TileSet
        foreach (var t in tileSet.GetComponentsInChildren<Tile>())
        {
            // Create a tile button for this guy
            var tileButton = Instantiate(TemplateTileButton);
            // Get a reference to the button's Button
            var tileButtonBtn = tileButton.GetComponent<Button>();
            // Get a reference to the button's Image
            var tileButtonImage = tileButton.GetComponent<Image>();
            // Get a reference tot he button's RectTransform
            var tileButtonRTf = tileButton.GetComponent<RectTransform>();

            // Get a reference to the tile Template's Sprite Renderer
            var tileSpriteRenderer = t.GetComponent<SpriteRenderer>();

            // Register the Callback
            Tile thisTile = t;
            tileButtonBtn.onClick.AddListener(delegate { LevelEditorReference.TileSelected(thisTile); });

            // Update the button's image
            tileButtonImage.sprite = tileSpriteRenderer.sprite;
            tileButtonImage.color = tileSpriteRenderer.color;

            // Update the button's alpha based on collision
            if (t.GetComponent<Collider2D>() == null)
            {
                // First, I have to get the color block, ok....
                var newCols = tileButton.colors;
                // Then I have to get the color from the color block?????
                var newNormalCol = newCols.normalColor;
                // Finally I can set the alpha level
                newNormalCol.a = PassThroughTileAlphaLevel;
                // And let's do it all over again for the highlights
                var newHighlightCol = newCols.highlightedColor;
                // Set the alpha level.. sigh...
                newHighlightCol.a = PassThroughTileAlphaLevel;
                // I HAVE TO STORE IT INTO THE COLOR BLOCK AGAIN?
                newCols.normalColor = newNormalCol;
                newCols.highlightedColor = newHighlightCol;
                // -.- All this just to set the alpha dynamically. Such a pain.
                tileButton.colors = newCols;
            }

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
