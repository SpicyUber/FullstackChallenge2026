using UnityEngine;

public class StartGameOnClick : MonoBehaviour, IClickHandler
{
    [SerializeField]
    public bool NewGame;
    [SerializeField]
    public bool Endless;

    public int Priority => 1;

    public void Handle()
    {
        GameManager.Instance.StartGame(Endless,NewGame);
    }
}
