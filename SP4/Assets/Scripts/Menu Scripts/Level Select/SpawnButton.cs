using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SpawnButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var info = new DirectoryInfo("Assets/Scenes/GameScenes");
        var files = info.GetFiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
