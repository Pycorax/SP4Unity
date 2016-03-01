using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayScore : MonoBehaviour
{
    Text text;

    // Use this for initialization
    void Start()
    {
        // Set up the reference.
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ScoreManager.CurrentScore.ToString();
    }
}
