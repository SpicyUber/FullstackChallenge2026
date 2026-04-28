using UnityEngine;

public interface IClickHandler
{
    public void Handle();

    /// <summary>
    /// Lower number gets applied earlier.
    /// </summary>
    public int Priority { get; }
}
