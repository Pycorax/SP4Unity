using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageScoreDisplay : MonoBehaviour {

    public Text text;

    private int score;
    
	// Use this for initialization
	void Start () {

        //text = GetComponent<Text>();

        score = ScoreManager.CurrentScore;

	}
	
	// Update is called once per frame
	void Update () {
        text.text = score.ToString();
	}
}
