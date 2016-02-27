using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SpawnButton : MonoBehaviour {

    public Transform button;

    private int fileCount;

    
	// Use this for initialization
	void Start () {
        var info = new DirectoryInfo("Assets/Scenes/GameScenes");
        var files = info.GetFiles();
        foreach (var i in files)
        {
            fileCount++;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadButtons()
    {
        Debug.Log(fileCount);
    }
}
