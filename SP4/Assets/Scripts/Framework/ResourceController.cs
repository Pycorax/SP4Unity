using System;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    // Defines the types of Resources Available
    public enum Resource
    {
        Explosion,
        Enemy
    }

    // Initial Initialization of Managers via Inspector
    public ResourceManager[] Managers = new ResourceManager[Enum.GetNames(typeof(Resource)).Length];

    // Stores the ResourceManagers
    private ResourceManager[] managers = new ResourceManager[Enum.GetNames(typeof(Resource)).Length];

    // Getters
    // -- Handle to the Explosion Manager
    public ResourceManager ExplosionManager { get { return managers[(int)Resource.Explosion]; } }
    // -- Handle to the Enemy Manager
    public ResourceManager EnemyManager { get { return managers[(int)Resource.Enemy]; } }

    void Start()
    {
        // Copy over the data from the public field to the private safer field
        for (int i = 0; i < Enum.GetNames(typeof(Resource)).Length; i++)
        {
            managers[i] = Managers[i];
        }

        // Clear the Managers
        Managers = null;
    }
}
