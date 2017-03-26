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

        float buttonSize = 1f;

        float yOffset = -20;

        int fileCount = 0;
        foreach(var i in files)
        {
            
            if (i.Extension == ".map")
            {
                //Instantiate Button
                var newButton = Instantiate(button);

                //Set Button Parent
                newButton.transform.SetParent(this.transform, false);

                newButton.transform.localPosition = new Vector3(-120, yOffset, 0);

                newButton.transform.localScale = new Vector3(buttonSize, buttonSize + .1f, buttonSize);
                    
                var text = newButton.GetComponentInChildren<Text>();

                text.text = Path.GetFileNameWithoutExtension(i.Name);
                
                newButton.onClick.AddListener(() =>
                    {
                        PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Level), text.text);
                    }
                    );


                ++fileCount;

                yOffset -= 100;
            }
        }

        // Set scroll rect to proper size
        rtransform.sizeDelta = new Vector2(40, 100 * fileCount);



    }

}
