public interface IPausable
{
    /// <summary>
    /// Define what to do when this object is to be paused.
    /// </summary>
    void Pause();
    /// <summary>
    /// Define what to do when this object is to be unpaused.
    /// </summary>
    void Unpause();
}