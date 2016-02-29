using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SpawnButton : MonoBehaviour {

    public Button button;

    private RectTransform rtransform;

	// Use this for initialization
	void Start () {
        rtransform = GetComponent<RectTransform>();
        LoadFiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadFiles()
    {
        var info = new DirectoryInfo("Assets/Maps");

        var files = info.GetFiles();

        float buttonSize = 0.7f;

        float yOffset = -50f;

        rtransform.sizeDelta = new Vector2(100, 50 * files.Length);
        
        foreach(var i in files)
        {
            
            if (i.Extension == ".map")
            {
                //Instantiate Button
                var newButton = Instantiate(button);

                //Set Button Parent
                newButton.transform.SetParent(this.transform);

                newButton.transform.localPosition = new Vector3(0, yOffset, 0);

                newButton.transform.localScale = new Vector3(buttonSize, buttonSize + 0.3f, buttonSize);

                

                
                var text = newButton.GetComponentInChildren<Text>();
                text.text = Path.GetFileNameWithoutExtension(i.Name);

                yOffset -= 120;
            }
        }

        

        

    }

}
