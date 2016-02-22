public class PausableResourceManager<T>  : ResourceManager<T>
    where T : IResourceManagable, IPausable, new()
{
    /// <summary>
    /// Pauses all the objects in the Resource List
    /// </summary>
    public void Pause()
    {
        foreach (var item in resourceList)
        {
            item.Pause();
        }
    }

    /// <summary>
    /// Unpauses all the objects in the Resource List
    /// </summary>
    public void Unpause()
    {
        foreach (var item in resourceList)
        {
            item.Unpause();
        }
    }
}
