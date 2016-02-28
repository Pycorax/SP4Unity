using System;
using UnityEngine;
using System.Collections;

public abstract class Objectives : MonoBehaviour
{
    [Tooltip("Reference to the GameManager for statistics.")]
    public GameManager Manager;

    private bool finished = false;

    /// <summary>
    /// Stores all the possible types of Objectives
    /// </summary>
    public enum Type
    {
        KillAll,
        CollectCoins,
        NoDamage
    }

    /// <summary>
    /// Do objective initialiation code here.
    /// </summary>
    protected virtual void Start()
    {

    }

    /// <summary>
    /// Do objective updates here
    /// </summary>
    protected virtual void Update()
    {
        if (!finished && IsAchieved())
        {
            finish();

            // Flag it so that we don't keep finishing it
            finished = true;
        }
    }

    public bool ParseParamString(string str)
    {
        var parameters = str.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);

        return parseParamString(parameters);
    }

    /// <summary>
    /// This function should define the end conditons of this objective.
    /// </summary>
    /// <returns></returns>
    public abstract bool IsAchieved();

    /// <summary>
    /// Call this function to do any clean up on the Objective once it has been completed
    /// </summary>
    protected abstract void finish();

    /// <summary>
    /// This function should initialize a Objective using a string provided.
    /// </summary>
    /// <param name="str">Initialization string.</param>
    /// <returns>Whether initialization was successful.</returns>
    protected abstract bool parseParamString(string[] parameters);
}
 