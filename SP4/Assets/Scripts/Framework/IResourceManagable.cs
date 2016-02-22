/// <summary>
/// Implement this interface if you wish to use a type with ResourceManager
/// </summary>
public interface IResourceManagable
{
    /// <summary>
    /// Function will be called prior to being stored in the resource pool. Use this to do initialization.
    /// </summary>
    void PrepareForResourcePool();
    /// <summary>
    /// Function for ResourceManager to check if this object is available to be plucked from the ResourceManager..
    /// </summary>
    /// <returns></returns>
    bool IsUsed();
    /// <summary>
    /// Function to set the object availability to be plucked from the ResourceManager.
    /// </summary>
    /// <param name="active"></param>
    void SetUsed(bool beingUsed);
}