using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrintObjective : MonoBehaviour
{
    public GameManager manager;
    Text text;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set up the reference.
        text = GetComponent<Text>();
        text.text = manager.CurrentObjective.GetDescription().ToString();
    }
}
