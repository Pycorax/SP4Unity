using System.Collections.Generic;

/// <summary>
/// A class that manages a list of resources in a form of a Resource Pool.
/// </summary>
/// <typeparam name="T">The type of resource you wish ResourceManager to manage.</typeparam>
public class ResourceManager<T>
    where T : IResourceManagable, new()
{
    // List Descriptors
    private uint resourceCap;           // The max number of items in the resource manager
    private bool expandable = true;            
    private uint expandSize = 10;            

    // List of Resources
    private List<T> resourceList = new List<T>();

    // Properties
    public uint ResourceCap { get { return resourceCap; } }             
    public bool Expandable { get; set; }                                // Dictates if the list can expand
    public uint ExpandSize { get; set; }                                // Dictates the amount we expand each time we need to

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="_resourceCap">The number of resources to cap the item at.</param>
    public ResourceManager(uint _resourceCap = 10)
    {
        resourceCap = _resourceCap;

        // Fill the list
        expand();
    }

    /// <summary>
    /// Fetches a resource from the resource manager.
    /// </summary>
    /// <returns>Returns a free resource.</returns>
    public T Fetch()
    {
        // Look in the current list for one
        foreach (var item in resourceList)
        {
            if (item.IsUsed() == false)
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
            return default(T);
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
        if (newSize < resourceCap)
        {
            return false;
        }

        // If not, let's raise the cap
        resourceCap = newSize;
        return true;
    }

    public void Reserve(uint reserveNumber)
    {
        // Do not allow reserving more than the cap
        if (reserveNumber > resourceCap)
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

    private bool expand()
    {
        // Do not allow expansion if expandable is set to false BUT allow first time expansion
        if (resourceList.Count == 0 || expandable)
        {
            uint expandBy = expandSize;

            // Check if we are unable to expand
            if (resourceList.Count > resourceCap)
            {
                return false;
            }
            // Check if we can expand but if we do we will burst the resource cap
            else if (resourceList.Count + expandSize > resourceCap)
            {
                // Calculate how much we can expand by
                expandBy = (uint)((int)resourceCap - resourceList.Count);
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
        T newObj = new T();
        // Prepare the resource
        newObj.PrepareForResourcePool();
        // Set it's activity to false
        newObj.SetUsed(false);
        // Add into the list
        resourceList.Add(newObj);
    }
}