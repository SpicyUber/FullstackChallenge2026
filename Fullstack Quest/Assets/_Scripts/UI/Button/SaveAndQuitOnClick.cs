using UnityEngine;

public class SaveAndQuitOnClick : MonoBehaviour, IClickHandler
{
    public int Priority => 0;

    public void Handle()
    {
        GameManager.Instance.SaveAndQuit();
    }
}
