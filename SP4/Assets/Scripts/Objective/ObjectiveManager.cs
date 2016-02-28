using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{

    public Objectives[] objectives;
    public GameManager manager;
    private bool objectivecompletedbefore;

    void Start()
    {
        objectives = GetComponentsInChildren<Objectives>();
        //if(objectives != null)
        //{
        //    Debug.Log("OBJECTIVE GIVEN");
        //}
    }

    void Update()
    {
        
    }
}
