using UnityEngine;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{

    public Objectives[] objectives;

    void Start()
    {
        objectives = GetComponents<Objectives>();
    }

    void Update()
    {
        foreach (var objective in objectives)
        {
            if (objective.IsAchieved())
            {
                objective.Complete();
                Destroy(objective);
            }
        }
    }

}
