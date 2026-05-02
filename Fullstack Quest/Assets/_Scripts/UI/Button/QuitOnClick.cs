using UnityEngine;

public class QuitOnClick : MonoBehaviour, IClickHandler
{
    public int Priority => 999;

    public void Handle() => Application.Quit();
}
