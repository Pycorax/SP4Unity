using UnityEngine;
using System.Collections;

public class LevelEditorCategoryButtonsController : MonoBehaviour
{
    [Tooltip("Reference to a GameObject that has TileSets that are categorized.")]
    public GameObject CategorizedTileSet;

    [Tooltip("Reference to a template TileButtons group to generate from.")]
    public GameObject TileButtonsTemplate;

	// Use this for initialization
	void Start ()
    {
        // For Every Category
	    foreach (Transform cat in CategorizedTileSet.transform)
	    {
	        // Generate a TileButtons set
	        var tileButtonsSet = Instantiate(TileButtonsTemplate.gameObject);

            // Give it a name
	        tileButtonsSet.name = cat.name;

            // Put it under me
            tileButtonsSet.transform.SetParent(transform);

            // Set the position
            tileButtonsSet.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0.0f, 60.0f);

            // Get a reference to the LevelEditorButtonsController
	        var btnController = tileButtonsSet.GetComponentInChildren<LevelEditorButtonsController>();

            // We found one?
	        if (btnController != null)
	        {
	            // Initialize it
                btnController.Load(cat.gameObject);
	        }

            // Set it to inactive
            tileButtonsSet.SetActive(false);
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
