using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoinsText : MonoBehaviour
{
    [Tooltip("Handle to the PlayerSettings to take in the number of coins.")]
    public PlayerSettings PlayerSetting;

    // Components
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

	// Update is called once per frame
	void Update ()
	{
	    text.text = "Coins: " + PlayerSetting.Coins;
	}
}
