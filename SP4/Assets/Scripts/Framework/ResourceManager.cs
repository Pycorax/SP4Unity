using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class that manages a list of resources in a form of a Resource Pool.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    // List Descriptors    
    [Tooltip("The maximum number of items in the resource manager.")]
    public uint ResourceCap = 10;
    [Tooltip("Determines if this ResourceManager can expand and grow when it has run out of inactive objects to fetch.")]
    public bool Expandable = true;
    [Tooltip("Dictates the amount to expand each time.")]
    public uint ExpandSize = 10;

    // Preloading
    [Tooltip("Preloads 'ExpandSize' number of items at the start. If 'PreloadAll' is true, this option is overriden.")]
    public bool PreloadOnce = false;
    [Tooltip("Preloads all items at the start. This option overrides 'PreloadOnce' if it is set to true.")]
    public bool PreloadAll = false;

    // Item Template
    [Tooltip("The item template. The resource of this list.")]
    public GameObject ItemTemplate;

    // Reorganization Delay
    [Tooltip("ResourceManager will check the list and return inactive objects that are not it's children back to being it's children for Scene Organization. This is the time between checks.")]
    public float TimeBetweenOrganization = 5.0f;
    private float organizationTimer = 0.0f;                 // Timer for Scene Organization

    // List of Resources
    protected List<GameObject> resourceList = new List<GameObject>();

    // List Access
    /// <summary>
    /// Get a list of all resources in this ResourceManager
    /// </summary>
    public List<GameObject> ResourceList { get { return resourceList; } }
    /// <summary>
    /// Get a list of active resources in this ResourceManager
    /// </summary>
    public List<GameObject> ActiveResourcesList
    {
        get
        {
            return (from item in resourceList where item.activeSelf select item).ToList();
        }
    }
    /// <summary>
    /// Get a list of inactive resources in this ResourceManager
    /// </summary>
    public List<GameObject> InactiveResourcesList
    {
        get
        {
            return (from item in resourceList where !item.activeSelf select item).ToList();
        }
    }

    void Start()
    {
        // If preload is enabled, reserve all the necessary items
        if (PreloadAll)
        {
            Reserve(ResourceCap);
        }
        else if (PreloadOnce)
        {
            expand();
        }
    }

    void Update()
    {
        // Update the Scene Organization timer
        organizationTimer += (float)TimeManager.GetDeltaTime(TimeManager.TimeType.SceneOrganization);

        // If it is time...
        if (organizationTimer > TimeBetweenOrganization)
        {
            // Organize the Scene
            foreach (var item in InactiveResourcesList)
            {
                // Force it to be the Resource Manager's child
                item.transform.parent = transform;
            }

            // Reset the timer
            organizationTimer = 0.0f;
        }
    }

    /// <summary>
    /// Fetches a resource from the resource manager.
    /// </summary>
    /// <returns>Returns a free resource.</returns>
    public GameObject Fetch()
    {
        // Look in the current list for one
        foreach (var item in resourceList)
        {
            if (item.activeSelf == false)
            {
                return item;
            }
        }

        // If we get here, means the list of inactive items is empty so let's make more
        if (expand())
        {
            return resourceList[resourceList.Count - 1];
        }
        else
        {
            // Returns null or 0 values if we are unable to get an item
            return null;
        }
    }

    /// <summary>
    /// Use this function to increase the size of the Resource Library. Attempting to decrease the size will
    /// result in false being returned.
    /// </summary>
    /// <param name="newSize">The new size to increase the maximum resource cap.</param>
    /// <returns>Whether it was successfully increased.</returns>
    public bool RaiseCap(uint newSize)
    {
        // Do not allow shrinking as it will cause problems with existing items
        if (newSize < ResourceCap)
        {
            return false;
        }

        // If not, let's raise the cap
        ResourceCap = newSize;
        return true;
    }

    /// <summary>
    /// Ensures there are a specified number of items in the list.
    /// </summary>
    /// <param name="reserveNumber">The number of items that should be in the list.</param>
    public void Reserve(uint reserveNumber)
    {
        // Do not allow reserving more than the cap
        if (reserveNumber > ResourceCap)
        {
            return;
        }

        // Calculate how many more we need to create
        uint toCreate = (uint)(reserveNumber - resourceList.Count);

        // Create the amount needed
        for (int i = 0; i < toCreate; i++)
        {
            addItemToList();
        }
    }

    /// <summary>
    /// Adds 'ExpandSize' amount of items into the list or until the list is full.
    /// </summary>
    /// <returns>Whether expansion occured.</returns>
    private bool expand()
    {
        // Do not allow expansion if expandable is set to false BUT allow first time expansion
        if (resourceList.Count == 0 || Expandable)
        {
            uint expandBy = ExpandSize;

            // Check if we are unable to expand
            if (resourceList.Count > ResourceCap)
            {
                return false;
            }
            // Check if we can expand but if we do we will burst the resource cap
            else if (resourceList.Count + ExpandSize > ResourceCap)
            {
                // Calculate how much we can expand by
                expandBy = (uint)((int)ResourceCap - resourceList.Count);
            }

            // Expand the list
            for (int i = 0; i < expandBy; i++)
            {
                addItemToList();
            }

            return true;
        }

        return false;
    }

    private void addItemToList()
    {
        // Create the resource
        GameObject newObj = Instantiate(ItemTemplate);
        // Set it's activity to false
        newObj.SetActive(false);
        // Move this object under the Manager's fold for Scene Organization
        newObj.transform.parent = transform;
        // Add into the list
        resourceList.Add(newObj);
    }
}