using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SpawnButton : MonoBehaviour {

    public Button button;
    
	// Use this for initialization
	void Start () {
        LoadFiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadFiles()
    {
        var info = new DirectoryInfo("Assets/Scenes/GameScenes");
        var files = info.GetFiles();

        foreach(var i in files)
        {
            button = GetComponent<Button>();
            var text = button.GetComponentInChildren<Text>();
            text.text = i.Name;
        }

    }

}
