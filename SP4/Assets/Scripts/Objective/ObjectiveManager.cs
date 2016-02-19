﻿using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{

    public Objectives[] objectives;
    public GameManager manager;

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
        foreach (var objective in objectives)
        {
            if (manager.LevelEnded == true)
            {
                if (objective.IsAchieved())
                {
                    objective.Complete();
                    Destroy(objective);
                }
            }
        }
    }
}
