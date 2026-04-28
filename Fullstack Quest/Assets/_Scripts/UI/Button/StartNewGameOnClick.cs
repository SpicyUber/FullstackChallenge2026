using UnityEngine;

public class StartNewGameOnClick : MonoBehaviour, IClickHandler
{
    public int Priority => 1;

    public void Handle()
    {
        GameManager.Instance.StartNewGame();
    }
}
