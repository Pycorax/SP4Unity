﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelEditorObjectiveDropdown : MonoBehaviour
{
#if UNITY_5_2_0
    // Component
    private Dropdown dropdown;

    // Getters
    public Objectives.Type Objective { get { return (Objectives.Type)dropdown.value; } }

	// Use this for initialization
	void Start ()
	{
	    dropdown = GetComponent<Dropdown>();

	    foreach (var v in Enum.GetNames(typeof (Objectives.Type)))
	    {
	        dropdown.options.Add(new Dropdown.OptionData(v));
	    }
	}
	
#else
    public Objectives.Type Objective { get { return Objectives.Type.CollectCoins; } }
#endif
    // Update is called once per frame
    void Update () {
	
	}
}
