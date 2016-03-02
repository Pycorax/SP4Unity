using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuPanelScript : MonoBehaviour {

    public Text text;

    public KeyCode enter = KeyCode.KeypadEnter;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(enter))
        {
            PlayerPrefs.SetString(SaveClass.GetKey(SaveClass.Save_Keys.Key_Coins), text.text);
            this.gameObject.SetActive(false);
        }
	}
}
